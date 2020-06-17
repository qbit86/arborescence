namespace Ubiquitous.Collections
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public ref struct ValueStack<T>
    {
        private const int DefaultCapacity = 4;

        private T[]? _arrayFromPool;
        private int _count;

        private static ArrayPool<T> Pool => ArrayPool<T>.Shared;

        public bool IsEmpty => _count == 0;

        public void Dispose()
        {
            _count = 0;
            T[]? arrayFromPool = _arrayFromPool;
            _arrayFromPool = null;
            if (arrayFromPool is null)
                return;

            Pool.Return(arrayFromPool, ShouldClear());
        }

        public void Add(T item)
        {
            _arrayFromPool ??= Pool.Rent(DefaultCapacity);

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
        private void ResizeThenAdd(T item)
        {
            Debug.Assert(_arrayFromPool != null, "_arrayFromPool != null");
            Debug.Assert(_arrayFromPool!.Length > 0, "_arrayFromPool.Length > 0");

            int count = _count;
            int newCapacity = count << 1;
            T[] newArray = Pool.Rent(newCapacity);
            Array.Copy(_arrayFromPool!, newArray, count);
            newArray[count] = item;

            T[] arrayFromPool = _arrayFromPool;
            _arrayFromPool = newArray;
            Pool.Return(arrayFromPool!, ShouldClear());
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
