namespace Ubiquitous.Collections
{
    using System;
    using System.Buffers;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public ref struct ValueStack<T>
    {
        private T[]? _arrayFromPool;
        private int _count;

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        public bool IsEmpty => _count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            CheckedReturnArray();
            _count = 0;
        }

        public void Add(T item)
        {
            int count = _count;
            T[] array = _arrayFromPool ?? Array.Empty<T>();

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

        public bool TryTake([MaybeNullWhen(false)] out T result)
        {
            int newCount = _count - 1;
            T[] array = _arrayFromPool ?? Array.Empty<T>();

            if ((uint)newCount >= (uint)array.Length)
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ResizeThenAdd(T item) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckedReturnArray()
        {
            if (_arrayFromPool != null)
            {
                Pool.Return(_arrayFromPool, ShouldClear());
                _arrayFromPool = null;
            }
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
