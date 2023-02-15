namespace Arborescence.Models
{
    using System.Collections.Generic;

    public readonly struct ListDictionaryMultimapPolicy<T, TMultimap> :
        IMultimapPolicy<T, TMultimap, List<T>.Enumerator>
        where TMultimap : IReadOnlyDictionary<T, List<T>>
    {
        private static readonly ListEnumerablePolicy<T> s_collectionPolicy = default;

        public List<T>.Enumerator GetEnumerator(TMultimap multimap, T key) =>
            MultimapHelpers<List<T>, List<T>.Enumerator>.GetEnumerator(multimap, key, s_collectionPolicy);
    }
}
