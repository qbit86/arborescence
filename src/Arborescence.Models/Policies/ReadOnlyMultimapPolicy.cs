namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyMultimapPolicyFactory<TKey, TValueCollection, TValueEnumerator, TMultimap>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
    {
        public static ReadOnlyMultimapPolicy<
            TKey, TValueCollection, TValueEnumerator, TMultimap, TCollectionPolicy> Create<TCollectionPolicy>(
            TCollectionPolicy collectionPolicy)
            where TCollectionPolicy : IEnumerablePolicy<TValueCollection, TValueEnumerator>
        {
            if (collectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(collectionPolicy));

            return new(collectionPolicy);
        }
    }

    public readonly struct ReadOnlyMultimapPolicy<
        TKey, TValueCollection, TValueEnumerator, TMultimap, TCollectionPolicy> :
        IReadOnlyMultimapPolicy<TKey, TMultimap, TValueEnumerator>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
        where TCollectionPolicy : IEnumerablePolicy<TValueCollection, TValueEnumerator>
    {
        private readonly TCollectionPolicy _collectionPolicy;

        internal ReadOnlyMultimapPolicy(TCollectionPolicy collectionPolicy) =>
            _collectionPolicy = collectionPolicy;

        public TValueEnumerator EnumerateValues(TMultimap multimap, TKey key) =>
            MultimapHelpers<TValueCollection, TValueEnumerator>.GetEnumerator(multimap, key, _collectionPolicy);

        public int GetCount(TMultimap multimap) => multimap.Count;
    }
}
