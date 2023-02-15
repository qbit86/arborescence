namespace Arborescence.Models
{
    using System.Collections.Generic;

    public readonly struct ListDictionaryMultimapPolicy<TKey, TValue, TMultimap> :
        IMultimapPolicy<TKey, TMultimap, List<TValue>.Enumerator>
        where TMultimap : IReadOnlyDictionary<TKey, List<TValue>>
    {
        private static readonly ListEnumerablePolicy<TValue> s_collectionPolicy = default;

        public List<TValue>.Enumerator GetEnumerator(TMultimap multimap, TKey key) =>
            multimap.TryGetValue(key, out List<TValue>? values)
                ? s_collectionPolicy.GetEnumerator(values)
                : s_collectionPolicy.GetEmptyEnumerator();
    }
}
