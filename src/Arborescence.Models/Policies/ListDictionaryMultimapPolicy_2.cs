namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal readonly struct ListDictionaryMultimapPolicy<T, TMultimap> :
        IMultimapPolicy<T, TMultimap, List<T>.Enumerator>
        where TMultimap : IReadOnlyDictionary<T, List<T>>
    {
        private static ListEnumerablePolicy<T> CollectionPolicy => default;

        public List<T>.Enumerator GetEnumerator(TMultimap multimap, T key) =>
            MultimapHelpers<List<T>, List<T>.Enumerator>.GetEnumerator(multimap, key, CollectionPolicy);

        public int GetCount(TMultimap multimap) => multimap.Count;
    }
}
