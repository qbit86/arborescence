namespace Arborescence.Models
{
    using System;
    using System.Buffers;
    using System.Diagnostics;

    internal static class ArraySegmentBuilder
    {
        private const int DefaultCapacity = 4;
        private const int MaxCoreClrArrayLength = 0x7fefffff;

        internal static ArraySegment<T> Add<T>(ArraySegment<T> arraySegment, T item, bool clearArray)
        {
            int capacity = arraySegment.Array is { } array ? array.Length : 0;

            if (arraySegment.Count == capacity)
                UncheckedEnsureCapacity(ref arraySegment, capacity, arraySegment.Count + 1, clearArray);

            UncheckedAdd(ref arraySegment, item);
            return arraySegment;
        }

        internal static ArraySegment<T> EnsureSize<T>(ArraySegment<T> arraySegment, int size, bool clearArray)
        {
            if (size <= arraySegment.Count)
                return arraySegment;

            UncheckedGrow(ref arraySegment, size, clearArray);
            return arraySegment;
        }

        private static void UncheckedGrow<T>(ref ArraySegment<T> arraySegment, int size, bool clearArray)
        {
            Debug.Assert(arraySegment.Count < size);

            int capacity = arraySegment.Array is { } array ? array.Length : 0;
            if (capacity < size)
                UncheckedEnsureCapacity(ref arraySegment, capacity, size, clearArray);

            int oldCount = arraySegment.Count;
            Array.Clear(arraySegment.Array!, oldCount, size - oldCount);
            arraySegment = new(arraySegment.Array!, 0, size);
        }

        private static void UncheckedAdd<T>(ref ArraySegment<T> arraySegment, T item)
        {
            T[] array = arraySegment.Array!;
            Debug.Assert(arraySegment.Count < array.Length);

            array[arraySegment.Count] = item;
            arraySegment = new(array, 0, arraySegment.Count + 1);
        }

        private static void UncheckedEnsureCapacity<T>(
            ref ArraySegment<T> arraySegment, int capacity, int minimum, bool clearArray)
        {
            Debug.Assert(minimum > capacity, "minimum > capacity");

            int nextCapacity = capacity == 0 ? DefaultCapacity : unchecked(2 * capacity);

            if (unchecked((uint)nextCapacity > MaxCoreClrArrayLength))
                nextCapacity = Math.Max(capacity + 1, MaxCoreClrArrayLength);

            nextCapacity = Math.Max(nextCapacity, minimum);

            T[] next = ArrayPool<T>.Shared.Rent(nextCapacity);
            if (arraySegment.Count > 0)
            {
                Array.Copy(arraySegment.Array!, 0, next, 0, arraySegment.Count);
                ArrayPool<T>.Shared.Return(arraySegment.Array!, clearArray);
            }

            arraySegment = new(next, 0, arraySegment.Count);
        }
    }
}
