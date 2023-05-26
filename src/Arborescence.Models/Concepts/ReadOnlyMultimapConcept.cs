namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyMultimapPolicyFactory<TKey, TValueCollection, TValueEnumerator, TMultimap>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
    {
        public static ReadOnlyMultimapConcept<
            TKey, TValueCollection, TValueEnumerator, TMultimap, TCollectionPolicy> Create<TCollectionPolicy>(
            TCollectionPolicy collectionPolicy)
            where TCollectionPolicy : IEnumeratorProvider<TValueCollection, TValueEnumerator>
        {
            if (collectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(collectionPolicy));

            return new(collectionPolicy);
        }
    }

    public readonly struct ReadOnlyMultimapConcept<
        TKey, TValueCollection, TValueEnumerator, TMultimap, TCollectionPolicy> :
        IReadOnlyMultimapConcept<TKey, TMultimap, TValueEnumerator>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
        where TCollectionPolicy : IEnumeratorProvider<TValueCollection, TValueEnumerator>
    {
        private readonly TCollectionPolicy _collectionPolicy;

        internal ReadOnlyMultimapConcept(TCollectionPolicy collectionPolicy) =>
            _collectionPolicy = collectionPolicy;

        public TValueEnumerator EnumerateValues(TMultimap multimap, TKey key) =>
            ReadOnlyMultimapHelpers<TValueCollection, TValueEnumerator>.GetEnumerator(multimap, key, _collectionPolicy);

        public int GetCount(TMultimap multimap) => multimap.Count;
    }
}
