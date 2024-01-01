#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides an enumerator over the <see cref="T:T[]"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    public readonly struct ArrayEnumeratorProvider<T> : IEnumeratorProvider<T[], ArraySegment<T>.Enumerator>
    {
        /// <inheritdoc/>
        public ArraySegment<T>.Enumerator GetEnumerator(T[] collection) =>
            collection is not null ? new ArraySegment<T>(collection).GetEnumerator() : GetEmptyEnumerator();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<T>.Enumerator GetEmptyEnumerator() => ArraySegment<T>.Empty.GetEnumerator();
    }
}
#endif
