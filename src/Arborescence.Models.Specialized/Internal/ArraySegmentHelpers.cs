namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;

    internal static class ArraySegmentHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ArraySegment<T>.Enumerator EmptyEnumerator<T>() => ArraySegment<T>.Empty.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ArraySegment<T>.Enumerator GetEnumerator<T>(ArraySegment<T> arraySegment) =>
            arraySegment.GetEnumerator();
    }
}
