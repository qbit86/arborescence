namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

    internal static class ArrayPrefixBuilder<T>
    {
        private const int DefaultCapacity = 4;
        private const int MaxCoreClrArrayLength = 0x7fefffff; // For byte arrays the limit is slightly larger

        internal static T[] EmptyArray { get; } = new T[0];

        internal static void Add(ref ArrayPrefix<T> arrayPrefix, T item)
        {
            int capacity = arrayPrefix.Array?.Length ?? 0;

            if (arrayPrefix.Count == capacity)
                EnsureCapacity(ref arrayPrefix, arrayPrefix.Count + 1);

            UncheckedAdd(ref arrayPrefix, item);
        }

        private static void UncheckedAdd(ref ArrayPrefix<T> arrayPrefix, T item)
        {
            Assert(arrayPrefix.Array != null, "arrayPrefix.Array != null");
            Assert(arrayPrefix.Count < arrayPrefix.Array.Length, "arrayPrefix.Count < arrayPrefix.Array.Length");

            arrayPrefix.Array[arrayPrefix.Count] = item;
            arrayPrefix = new ArrayPrefix<T>(arrayPrefix.Array, arrayPrefix.Count + 1);
        }

        private static void EnsureCapacity(ref ArrayPrefix<T> arrayPrefix, int minimum)
        {
            int capacity = arrayPrefix.Array?.Length ?? 0;

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
                ArrayPool<T>.Shared.Return(arrayPrefix.Array, true);
            }

            arrayPrefix = new ArrayPrefix<T>(next, arrayPrefix.Count);
        }
    }
}
