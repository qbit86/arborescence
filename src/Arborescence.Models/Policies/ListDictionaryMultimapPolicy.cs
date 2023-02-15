namespace Arborescence.Models
{
    using System.Collections.Generic;

    public readonly struct ListDictionaryMultimapPolicy<T, TMultimap> :
        IMultimapPolicy<T, TMultimap, List<T>.Enumerator>
        where TMultimap : IReadOnlyDictionary<T, List<T>>
    {
        private static readonly ListEnumerablePolicy<T> s_collectionPolicy = default;

        public List<T>.Enumerator GetEnumerator(TMultimap multimap, T key) =>
            multimap.TryGetValue(key, out List<T>? values)
                ? s_collectionPolicy.GetEnumerator(values)
                : s_collectionPolicy.GetEmptyEnumerator();
    }
}
