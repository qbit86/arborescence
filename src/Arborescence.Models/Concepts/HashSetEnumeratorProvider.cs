namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides an enumerator over the <see cref="HashSet{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public readonly struct HashSetEnumeratorProvider<T> : IEnumeratorProvider<HashSet<T>, HashSet<T>.Enumerator>
    {
        private static readonly HashSet<T> s_empty = new();

        /// <inheritdoc/>
        public HashSet<T>.Enumerator GetEnumerator(HashSet<T> collection) =>
            collection?.GetEnumerator() ?? GetEmptyEnumerator();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashSet<T>.Enumerator GetEmptyEnumerator() => s_empty.GetEnumerator();
    }
}
