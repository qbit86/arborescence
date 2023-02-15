namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class DictionaryMultimapPolicy<TKey, TValueCollection, TValueEnumerator, TMultimap>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
    {
        public static DictionaryMultimapPolicy<
            TKey, TValueCollection, TValueEnumerator, TMultimap, TCollectionPolicy> Create<TCollectionPolicy>(
            TCollectionPolicy collectionPolicy)
            where TCollectionPolicy : IEnumerablePolicy<TValueCollection, TValueEnumerator>
        {
            if (collectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(collectionPolicy));

            return new(collectionPolicy);
        }
    }

    public readonly struct DictionaryMultimapPolicy<
        TKey, TValueCollection, TValueEnumerator, TMultimap, TCollectionPolicy> :
        IMultimapPolicy<TKey, TMultimap, TValueEnumerator>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
        where TCollectionPolicy : IEnumerablePolicy<TValueCollection, TValueEnumerator>
    {
        private readonly TCollectionPolicy _collectionPolicy;

        internal DictionaryMultimapPolicy(TCollectionPolicy collectionPolicy) =>
            _collectionPolicy = collectionPolicy;

        public TValueEnumerator GetEnumerator(TMultimap multimap, TKey key) =>
            MultimapHelpers<TValueCollection, TValueEnumerator>.GetEnumerator(multimap, key, _collectionPolicy);
    }
}
