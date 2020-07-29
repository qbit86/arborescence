namespace Arborescence
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

    internal static class ArrayPrefixBuilder
    {
        private const int DefaultCapacity = 4;
        private const int MaxCoreClrArrayLength = 0x7fefffff; // For byte arrays the limit is slightly larger

        internal static ArrayPrefix<T> Create<T>(int capacity)
        {
            Assert(capacity >= 0, "capacity >= 0");

            T[] array = ArrayPool<T>.Shared.Rent(capacity);
            return ArrayPrefix.Create(array, 0);
        }

        internal static ArrayPrefix<T> Add<T>(ArrayPrefix<T> arrayPrefix, T item, bool clearArray)
        {
            int capacity = (arrayPrefix.Array?.Length).GetValueOrDefault();

            if (arrayPrefix.Count == capacity)
                UncheckedEnsureCapacity(ref arrayPrefix, capacity, arrayPrefix.Count + 1, clearArray);

            UncheckedAdd(ref arrayPrefix, item);
            return arrayPrefix;
        }

        internal static ArrayPrefix<T> Resize<T>(ArrayPrefix<T> arrayPrefix, int size, bool clearArray)
        {
            if (arrayPrefix.Count < size)
            {
                UncheckedGrow(ref arrayPrefix, size, clearArray);
                return arrayPrefix;
            }

            if (arrayPrefix.Count > size)
            {
                UncheckedShrink(ref arrayPrefix, size, clearArray);
                return arrayPrefix;
            }

            return arrayPrefix;
        }

        internal static ArrayPrefix<T> Release<T>(ArrayPrefix<T> arrayPrefix, bool clearArray)
        {
            if (arrayPrefix.Array is {} array)
                ArrayPool<T>.Shared.Return(array, clearArray);
            return ArrayPrefix<T>.Empty;
        }

        private static void UncheckedGrow<T>(ref ArrayPrefix<T> arrayPrefix, int size, bool clearArray)
        {
            Assert(arrayPrefix.Count < size, "arrayPrefix.Count < size");

            int capacity = (arrayPrefix.Array?.Length).GetValueOrDefault();
            if (capacity < size)
                UncheckedEnsureCapacity(ref arrayPrefix, capacity, size, clearArray);

            int oldCount = arrayPrefix.Count;
            Assert(arrayPrefix.Array != null, "arrayPrefix.Array != null");
            Array.Clear(arrayPrefix.Array, oldCount, size - oldCount);
            arrayPrefix = ArrayPrefix.Create(arrayPrefix.Array, size);
        }

        private static void UncheckedShrink<T>(ref ArrayPrefix<T> arrayPrefix, int size, bool clearArray)
        {
            Assert(arrayPrefix.Count > size, "arrayPrefix.Count > size");

            int oldCount = arrayPrefix.Count;
            arrayPrefix = ArrayPrefix.Create(arrayPrefix.Array, size);
            if (clearArray)
                Array.Clear(arrayPrefix.Array, size, oldCount - size);
        }

        private static void UncheckedAdd<T>(ref ArrayPrefix<T> arrayPrefix, T item)
        {
            Assert(arrayPrefix.Array != null, "arrayPrefix.Array != null");
            Assert(arrayPrefix.Count < arrayPrefix.Array.Length, "arrayPrefix.Count < arrayPrefix.Array.Length");

            arrayPrefix.Array[arrayPrefix.Count] = item;
            arrayPrefix = ArrayPrefix.Create(arrayPrefix.Array, arrayPrefix.Count + 1);
        }

        private static void UncheckedEnsureCapacity<T>(ref ArrayPrefix<T> arrayPrefix, int capacity, int minimum,
            bool clearArray)
        {
            Assert(minimum > capacity, "minimum > capacity");

            int nextCapacity = capacity == 0 ? DefaultCapacity : unchecked(2 * capacity);

            if ((uint)nextCapacity > MaxCoreClrArrayLength)
                nextCapacity = Math.Max(capacity + 1, MaxCoreClrArrayLength);

            nextCapacity = Math.Max(nextCapacity, minimum);

            T[] next = ArrayPool<T>.Shared.Rent(nextCapacity);
            if (arrayPrefix.Count > 0)
            {
                Assert(arrayPrefix.Array != null, "arrayPrefix.Array != null");
                Array.Copy(arrayPrefix.Array, 0, next, 0, arrayPrefix.Count);
                ArrayPool<T>.Shared.Return(arrayPrefix.Array, clearArray);
            }

            arrayPrefix = ArrayPrefix.Create(next, arrayPrefix.Count);
        }
    }
}
