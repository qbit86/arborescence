namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal readonly struct ListMultimapPolicy<T, TMultimap> :
        IReadOnlyMultimapPolicy<T, TMultimap, List<T>.Enumerator>,
        IDictionaryAddition<T, T, TMultimap>
        where TMultimap : IDictionary<T, List<T>>
    {
        private static ListEnumerablePolicy<T> CollectionPolicy => default;

        public List<T>.Enumerator EnumerateValues(TMultimap multimap, T key) =>
            MultimapHelpers<List<T>, List<T>.Enumerator>.GetEnumerator(multimap, key, CollectionPolicy);

        public int GetCount(TMultimap multimap) => multimap.Count;

        public void Add(TMultimap dictionary, T key, T value)
        {
            if (dictionary.TryGetValue(key, out List<T>? values))
                values.Add(value);
            else
                dictionary.Add(key, new() { value });
        }
    }
}
