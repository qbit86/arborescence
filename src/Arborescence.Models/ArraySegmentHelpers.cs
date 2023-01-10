namespace Arborescence.Models
{
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
#else
    using System.Collections.Generic;
    using System.Linq;
#endif
    using System;
    using System.Runtime.CompilerServices;

    internal static class ArraySegmentHelpers
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ArraySegment<T>.Enumerator EmptyEnumerator<T>() => ArraySegment<T>.Empty.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ArraySegment<T>.Enumerator GetEnumerator<T>(ArraySegment<T> arraySegment) =>
            arraySegment.GetEnumerator();
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerator<T> EmptyEnumerator<T>() => Enumerable.Empty<T>().GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerator<T> GetEnumerator<T>(ArraySegment<T> arraySegment) =>
            ((IEnumerable<T>)arraySegment).GetEnumerator();
#endif
    }
}
