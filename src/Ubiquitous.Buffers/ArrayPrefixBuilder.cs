namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

    internal static class ArrayPrefixBuilder
    {
        private const int DefaultCapacity = 4;
        private const int MaxCoreClrArrayLength = 0x7fefffff; // For byte arrays the limit is slightly larger

        internal static ArrayPrefix<T> Add<T>(ArrayPrefix<T> arrayPrefix, T item, bool clearArray)
        {
            int capacity = arrayPrefix.Array?.Length ?? 0;

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

        private static void UncheckedGrow<T>(ref ArrayPrefix<T> arrayPrefix, int size, bool clearArray)
        {
            Assert(arrayPrefix.Count < size, "arrayPrefix.Count < size");

            int capacity = arrayPrefix.Array?.Length ?? 0;
            if (capacity < size)
                UncheckedEnsureCapacity(ref arrayPrefix, capacity, size, clearArray);

            int oldCount = arrayPrefix.Count;
            Array.Clear(arrayPrefix.Array, oldCount, size - oldCount);
            arrayPrefix = new ArrayPrefix<T>(arrayPrefix.Array, size);
        }

        private static void UncheckedShrink<T>(ref ArrayPrefix<T> arrayPrefix, int size, bool clearArray)
        {
            Assert(arrayPrefix.Count > size, "arrayPrefix.Count > size");

            int oldCount = arrayPrefix.Count;
            arrayPrefix = new ArrayPrefix<T>(arrayPrefix.Array, size);
            if (clearArray)
                Array.Clear(arrayPrefix.Array, size, oldCount - size);
        }

        private static void UncheckedAdd<T>(ref ArrayPrefix<T> arrayPrefix, T item)
        {
            Assert(arrayPrefix.Array != null, "arrayPrefix.Array != null");
            Assert(arrayPrefix.Count < arrayPrefix.Array.Length, "arrayPrefix.Count < arrayPrefix.Array.Length");

            arrayPrefix.Array[arrayPrefix.Count] = item;
            arrayPrefix = new ArrayPrefix<T>(arrayPrefix.Array, arrayPrefix.Count + 1);
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

            arrayPrefix = new ArrayPrefix<T>(next, arrayPrefix.Count);
        }
    }
}
