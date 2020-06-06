namespace Ubiquitous.Internal
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal struct Stack<T> : IDisposable
    {
        private const int DefaultCapacity = 4;

        private T[] _arrayFromPool;
        private int _count;

        private Stack(T[] array)
        {
            Debug.Assert(array != null, "array != null");
            _arrayFromPool = array;
            _count = 0;
        }

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        internal static Stack<T> Create() => new Stack<T>(Array.Empty<T>());

        public void Dispose()
        {
            _count = 0;
            if (_arrayFromPool.Length == 0)
                return;

            Pool.Return(_arrayFromPool, ShouldClear());
            _arrayFromPool = Array.Empty<T>();
        }

        internal void Add(T item)
        {
            int count = _count;
            T[] array = _arrayFromPool;

            if ((uint)count < (uint)array.Length)
            {
                array[count] = item;
                _count = count + 1;
            }
            else
            {
                ResizeThenAdd(item);
            }
        }

        internal bool TryTake(out T result)
        {
            int newCount = _count - 1;
            T[] array = _arrayFromPool;

            if ((uint)newCount >= (uint)array.Length)
            {
                result = default;
                return false;
            }

            _count = newCount;
            result = array[newCount];
            if (ShouldClear())
                array[newCount] = default;

            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ResizeThenAdd(T item)
        {
            int count = _count;
            int newCapacity = count == 0 ? DefaultCapacity : count << 1;
            T[] newArray = Pool.Rent(newCapacity);
            Array.Copy(_arrayFromPool, newArray, count);
            newArray[count] = item;
            if (_arrayFromPool.Length != 0)
                Pool.Return(_arrayFromPool, ShouldClear());
            _arrayFromPool = newArray;
            _count = count + 1;
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
