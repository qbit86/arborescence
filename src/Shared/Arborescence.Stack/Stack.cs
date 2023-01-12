namespace Arborescence.Internal
{
    using System;
    using System.Buffers;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal struct Stack<T> : IProducerConsumerCollection<T>, IDisposable
    {
        private const int DefaultCapacity = 4;

        private T[]? _arrayFromPool;
        private int _count;

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        public void Dispose()
        {
            T[]? arrayFromPool = _arrayFromPool;
            this = default;
            if (arrayFromPool is null)
                return;

            Pool.Return(arrayFromPool, ShouldClear());
        }

        internal void Add(T item)
        {
            _arrayFromPool ??= Pool.Rent(DefaultCapacity);

            int count = _count;
            T[] array = _arrayFromPool;

            if (unchecked((uint)count < (uint)array.Length))
            {
                array[count] = item;
                _count = count + 1;
            }
            else
            {
                ResizeThenAdd(item);
            }
        }

        public readonly void CopyTo(T[] array, int index) => throw new NotSupportedException();

        public readonly T[] ToArray() => throw new NotSupportedException();

        public bool TryAdd(T item)
        {
            Add(item);
            return true;
        }

        public bool TryTake(out T result)
        {
            int newCount = _count - 1;
            T[] array = _arrayFromPool ?? Array.Empty<T>();

            if (unchecked((uint)newCount >= (uint)array.Length))
            {
                result = default!;
                return false;
            }

            _count = newCount;
            result = array[newCount];
            if (ShouldClear())
                array[newCount] = default!;

            return true;
        }

        public readonly IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly void CopyTo(Array array, int index) => throw new NotSupportedException();

        public readonly int Count => _count;

        public readonly bool IsSynchronized => false;

        public readonly object SyncRoot => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ResizeThenAdd(T item)
        {
            T[] arrayFromPool = _arrayFromPool!;
            Debug.Assert(arrayFromPool.Length > 0, "arrayFromPool.Length > 0");

            int count = _count;
            int newCapacity = count << 1;
            T[] newArray = Pool.Rent(newCapacity);
            Array.Copy(arrayFromPool, newArray, count);
            newArray[count] = item;

            _arrayFromPool = newArray;
            Pool.Return(arrayFromPool, ShouldClear());
            _count = count + 1;
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
    }
}
