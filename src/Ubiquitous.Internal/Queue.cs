namespace Ubiquitous.Internal
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Collections;

    internal struct Queue<T> : IDisposable, IContainer<T>
    {
        private const int DefaultCapacity = 4;

        private T[] _arrayFromPool;
        private int _head;
        private int _tail;
        private int _size;

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        public bool IsEmpty => _size == 0;

        public void Dispose()
        {
            _head = 0;
            _tail = 0;
            _size = 0;
            if (_arrayFromPool is null)
                return;

            Pool.Return(_arrayFromPool, ShouldClear());
            _arrayFromPool = null;
        }

        public void Add(T item)
        {
            if (_arrayFromPool is null)
                _arrayFromPool = Pool.Rent(DefaultCapacity);

            if (_size == _arrayFromPool.Length)
            {
                int newCapacity = _size << 1;
                SetCapacity(newCapacity);
            }

            _arrayFromPool[_tail] = item;
            MoveNext(ref _tail);
            ++_size;
        }

        public bool TryTake(out T result)
        {
            if (_size == 0)
            {
                result = default;
                return false;
            }

            int head = _head;
            T[] array = _arrayFromPool ?? Array.Empty<T>();
            result = array[head];
            if (ShouldClear())
                array[head] = default;

            MoveNext(ref _head);
            --_size;
            return true;
        }

        // Increments the index wrapping it if necessary.
        private void MoveNext(ref int index)
        {
            int temp = index + 1;
            if (temp == _arrayFromPool.Length)
                temp = 0;

            index = temp;
        }

        private void SetCapacity(int capacity)
        {
            Debug.Assert(capacity > 0, "capacity > 0");

            T[] newArray = Pool.Rent(capacity);
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_arrayFromPool, _head, newArray, 0, _size);
                }
                else
                {
                    Array.Copy(_arrayFromPool, _head, newArray, 0, _arrayFromPool.Length - _head);
                    Array.Copy(_arrayFromPool, 0, newArray, _arrayFromPool.Length - _head, _tail);
                }
            }

            _arrayFromPool = newArray;
            _head = 0;
            _tail = _size == capacity ? 0 : _size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ShouldClear()
        {
#if NETSTANDARD2_1 || NETCOREAPP2_0
            return RuntimeHelpers.IsReferenceOrContainsReferences<T>();
#else
            return true;
#endif
        }
    }
}
