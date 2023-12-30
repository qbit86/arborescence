#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;

    public readonly struct ArrayEnumeratorProvider<T> : IEnumeratorProvider<T[], ArraySegment<T>.Enumerator>
    {
        public ArraySegment<T>.Enumerator GetEnumerator(T[] collection) =>
            collection is not null ? new ArraySegment<T>(collection).GetEnumerator() : GetEmptyEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<T>.Enumerator GetEmptyEnumerator() => ArraySegment<T>.Empty.GetEnumerator();
    }
}
#endif
