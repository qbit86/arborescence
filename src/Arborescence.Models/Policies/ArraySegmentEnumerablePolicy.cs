#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;

    public struct ArraySegmentEnumerablePolicy<T> : IEnumerablePolicy<ArraySegment<T>, ArraySegment<T>.Enumerator>
    {
        public ArraySegment<T>.Enumerator GetEnumerator(ArraySegment<T> collection) => collection.GetEnumerator();

        public ArraySegment<T>.Enumerator GetEmptyEnumerator() => ArraySegment<T>.Empty.GetEnumerator();
    }
}
#endif
