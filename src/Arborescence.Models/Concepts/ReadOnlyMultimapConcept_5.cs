namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides basic read-only methods for a generic multimap.
    /// </summary>
    /// <typeparam name="TMultimap">The type of the dictionary.</typeparam>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValueCollection">The type of the value collection.</typeparam>
    /// <typeparam name="TValueEnumerator">The type of the value enumerator.</typeparam>
    /// <typeparam name="TEnumeratorProvider">The type of the enumerator provider.</typeparam>
    public readonly struct ReadOnlyMultimapConcept<
        TMultimap, TKey, TValueCollection, TValueEnumerator, TEnumeratorProvider> :
        IReadOnlyMultimapConcept<TMultimap, TKey, TValueEnumerator>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
        where TEnumeratorProvider : IEnumeratorProvider<TValueCollection, TValueEnumerator>
    {
        private readonly TEnumeratorProvider _enumeratorProvider;

        internal ReadOnlyMultimapConcept(TEnumeratorProvider enumeratorProvider) =>
            _enumeratorProvider = enumeratorProvider;

        /// <inheritdoc/>
        public TValueEnumerator EnumerateValues(TMultimap multimap, TKey key) =>
            ReadOnlyMultimapHelpers<TValueCollection, TValueEnumerator>.GetEnumerator(
                multimap, key, _enumeratorProvider);

        /// <inheritdoc/>
        public int GetCount(TMultimap multimap) => multimap.Count;
    }
}
