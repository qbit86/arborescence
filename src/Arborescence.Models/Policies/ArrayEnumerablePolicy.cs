#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;

    public struct ArrayEnumerablePolicy<T> : IEnumerablePolicy<T[], ArraySegment<T>.Enumerator>
    {
        public ArraySegment<T>.Enumerator GetEnumerator(T[] collection) =>
            new ArraySegment<T>(collection).GetEnumerator();

        public ArraySegment<T>.Enumerator GetEmptyEnumerator() => ArraySegment<T>.Empty.GetEnumerator();
    }
}
#endif
