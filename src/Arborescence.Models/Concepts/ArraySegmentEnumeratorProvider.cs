#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;

    internal struct ArraySegmentEnumeratorProvider<T> : IEnumeratorProvider<ArraySegment<T>, ArraySegment<T>.Enumerator>
    {
        public ArraySegment<T>.Enumerator GetEnumerator(ArraySegment<T> collection) =>
            collection.Array is not null ? collection.GetEnumerator() : GetEmptyEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<T>.Enumerator GetEmptyEnumerator() => ArraySegment<T>.Empty.GetEnumerator();
    }
}
#endif
