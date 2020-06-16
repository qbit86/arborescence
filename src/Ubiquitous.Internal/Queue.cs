namespace Ubiquitous.Internal.Experimental
{
    using System;
    using System.Buffers;
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

            throw new NotImplementedException();
        }

        public bool TryTake(out T result)
        {
            throw new NotImplementedException();
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
