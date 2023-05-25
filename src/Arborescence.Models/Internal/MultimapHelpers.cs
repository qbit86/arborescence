namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal static class ReadOnlyMultimapHelpers<TValueCollection, TValueEnumerator>
    {
        internal static TValueEnumerator GetEnumerator<TKey, TMultimap, TCollectionPolicy>(
            TMultimap multimap, TKey key, TCollectionPolicy policy)
            where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
            where TCollectionPolicy : IEnumerablePolicy<TValueCollection, TValueEnumerator> =>
            multimap.TryGetValue(key, out TValueCollection? values)
                ? policy.GetEnumerator(values)
                : policy.GetEmptyEnumerator();
    }

    internal static class MultimapHelpers<TValueCollection, TValueEnumerator>
    {
        internal static TValueEnumerator GetEnumerator<TKey, TMultimap, TCollectionPolicy>(
            TMultimap multimap, TKey key, TCollectionPolicy policy)
            where TMultimap : IDictionary<TKey, TValueCollection>
            where TCollectionPolicy : IEnumerablePolicy<TValueCollection, TValueEnumerator> =>
            multimap.TryGetValue(key, out TValueCollection? values)
                ? policy.GetEnumerator(values)
                : policy.GetEmptyEnumerator();
    }
}
