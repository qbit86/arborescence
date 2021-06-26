namespace Arborescence.Internal
{
    using System;
    using System.Buffers;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal struct Queue<T> : IProducerConsumerCollection<T>, IDisposable
    {
        private const int DefaultCapacity = 4;

        private T[]? _arrayFromPool;
        private int _head;
        private int _tail;
        private int _size;

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        public void Dispose()
        {
            T[]? arrayFromPool = _arrayFromPool;
            this = default;
            if (arrayFromPool is null)
                return;

            Pool.Return(arrayFromPool, ShouldClear());
        }

        public void Add(T item)
        {
            _arrayFromPool ??= Pool.Rent(DefaultCapacity);

            if (_size == _arrayFromPool.Length)
            {
                int newCapacity = _size << 1;
                SetCapacity(newCapacity);
            }

            _arrayFromPool[_tail] = item;
            MoveNext(ref _tail);
            ++_size;
        }

        public void CopyTo(T[] array, int index) => throw new NotSupportedException();

        public T[] ToArray() => throw new NotSupportedException();

        public bool TryAdd(T item)
        {
            Add(item);
            return true;
        }

        public bool TryTake(out T result)
        {
            if (_size == 0)
            {
                result = default!;
                return false;
            }

            int head = _head;
            T[] array = _arrayFromPool ?? Array.Empty<T>();
            result = array[head];
            if (ShouldClear())
                array[head] = default!;

            MoveNext(ref _head);
            --_size;
            return true;
        }

        public IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(Array array, int index) => throw new NotSupportedException();

        public int Count => _size;

        public bool IsSynchronized => false;

        public object SyncRoot => throw new NotSupportedException();

        // Increments the index wrapping it if necessary.
        private void MoveNext(ref int index)
        {
            int temp = index + 1;
            if (temp == _arrayFromPool!.Length)
                temp = 0;

            index = temp;
        }

        private void SetCapacity(int capacity)
        {
            Debug.Assert(capacity > 0, nameof(capacity) + " > 0");

            T[] arrayFromPool = _arrayFromPool!;
            T[] newArray = Pool.Rent(capacity);
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(arrayFromPool, _head, newArray, 0, _size);
                }
                else
                {
                    Array.Copy(arrayFromPool, _head, newArray, 0, arrayFromPool.Length - _head);
                    Array.Copy(arrayFromPool, 0, newArray, arrayFromPool.Length - _head, _tail);
                }
            }

            _arrayFromPool = newArray;
            Pool.Return(arrayFromPool, ShouldClear());
            _head = 0;
            _tail = _size == capacity ? 0 : _size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ShouldClear()
        {
#if NETSTANDARD2_1 || NETCOREAPP2_1
            return RuntimeHelpers.IsReferenceOrContainsReferences<T>();
#else
            return true;
#endif
        }
    }
}
