#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides an enumerator over the <see cref="ArraySegment{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array segment.</typeparam>
    public readonly struct ArraySegmentEnumeratorProvider<T> :
        IEnumeratorProvider<ArraySegment<T>, ArraySegment<T>.Enumerator>
    {
        /// <inheritdoc/>
        public ArraySegment<T>.Enumerator GetEnumerator(ArraySegment<T> collection) =>
            collection.Array is not null ? collection.GetEnumerator() : GetEmptyEnumerator();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<T>.Enumerator GetEmptyEnumerator() => ArraySegment<T>.Empty.GetEnumerator();
    }
}
#endif
