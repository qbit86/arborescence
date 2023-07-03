namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides an enumerator over the <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public readonly struct ListEnumeratorProvider<T> : IEnumeratorProvider<List<T>, List<T>.Enumerator>
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private static readonly List<T> s_empty = new();

        /// <inheritdoc/>
        public List<T>.Enumerator GetEnumerator(List<T> collection) =>
            collection?.GetEnumerator() ?? GetEmptyEnumerator();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T>.Enumerator GetEmptyEnumerator() => s_empty.GetEnumerator();
    }
}
