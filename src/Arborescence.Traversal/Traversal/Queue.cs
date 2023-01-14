namespace Arborescence.Traversal
{
    using System;
    using System.Buffers;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal sealed partial class Queue<T> : IProducerConsumerCollection<T>, IDisposable
    {
        private const int DefaultCapacity = 4;

        private T[] _arrayFromPool = Array.Empty<T>();
        private int _head;
        private int _size;
        private int _tail;

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        public void Dispose()
        {
            ResetArray();
            _head = _tail = _size = 0;
        }

        public int Count => _size;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public bool TryAdd(T item)
        {
            if (_arrayFromPool.Length == 0)
                _arrayFromPool = Pool.Rent(DefaultCapacity);
            else if (_size == _arrayFromPool.Length)
                Grow(_size + 1);

            _arrayFromPool[_tail] = item;
            MoveNext(ref _tail);
            ++_size;
            return true;
        }

        public bool TryTake(out T item)
        {
            int head = _head;
            T[] array = _arrayFromPool;

            if (_size == 0)
            {
                item = default!;
                return false;
            }

            item = array[head];
            if (ShouldClear())
                array[head] = default!;

            MoveNext(ref _head);
            --_size;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ShouldClear()
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            return RuntimeHelpers.IsReferenceOrContainsReferences<T>();
#else
            return true;
#endif
        }

        private void SetCapacity(int capacity)
        {
            Debug.Assert(capacity > 0, nameof(capacity) + " > 0");
            T[] arrayFromPool = _arrayFromPool;
            T[] newArrayFromPool = Pool.Rent(capacity);
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(arrayFromPool, _head, newArrayFromPool, 0, _size);
                }
                else
                {
                    Array.Copy(arrayFromPool, _head, newArrayFromPool, 0, arrayFromPool.Length - _head);
                    Array.Copy(arrayFromPool, 0, newArrayFromPool, arrayFromPool.Length - _head, _tail);
                }
            }

            _arrayFromPool = newArrayFromPool;
            if (arrayFromPool.Length > 0)
                Pool.Return(arrayFromPool, ShouldClear());
            _head = 0;
            _tail = _size == capacity ? 0 : _size;
        }

        // Increments the index wrapping it if necessary.
        private void MoveNext(ref int index)
        {
            int temp = index + 1;
            if (temp == _arrayFromPool.Length)
                temp = 0;

            index = temp;
        }

        private void Grow(int capacity)
        {
            Debug.Assert(_arrayFromPool.Length < capacity);

            const int minimumGrow = 4;

            int newCapacity = _arrayFromPool.Length << 1;
            newCapacity = Math.Max(newCapacity, _arrayFromPool.Length + minimumGrow);
            if (newCapacity < capacity)
                newCapacity = capacity;

            SetCapacity(newCapacity);
        }

        private void ResetArray()
        {
            if (_arrayFromPool.Length == 0)
                return;
            T[] arrayFromPool = _arrayFromPool;
            _arrayFromPool = Array.Empty<T>();
            Pool.Return(arrayFromPool, ShouldClear());
        }
    }
}
