namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal readonly struct ListDictionaryMultimapPolicy<TKey, TValue, TMultimap> :
        IMultimapPolicy<TKey, TMultimap, List<TValue>.Enumerator>
        where TMultimap : IReadOnlyDictionary<TKey, List<TValue>>
    {
        private static ListEnumerablePolicy<TValue> CollectionPolicy => default;

        public List<TValue>.Enumerator GetEnumerator(TMultimap multimap, TKey key) =>
            MultimapHelpers<List<TValue>, List<TValue>.Enumerator>.GetEnumerator(multimap, key, CollectionPolicy);

        public int GetCount(TMultimap multimap) => multimap.Count;
    }
}
