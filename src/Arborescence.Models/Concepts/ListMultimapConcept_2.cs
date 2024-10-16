namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal readonly struct ListMultimapConcept<TMultimap, T> :
        IReadOnlyMultimapConcept<TMultimap, T, List<T>.Enumerator>,
        IDictionaryAddition<TMultimap, T, T>
        where TMultimap : IDictionary<T, List<T>>
    {
        private static ListEnumeratorProvider<T> EnumeratorProvider => default;

        public List<T>.Enumerator EnumerateValues(TMultimap multimap, T key) =>
            MultimapHelpers<List<T>, List<T>.Enumerator>.GetEnumerator(multimap, key, EnumeratorProvider);

        public int GetCount(TMultimap multimap) => multimap.Count;

        public void Add(TMultimap dictionary, T key, T value)
        {
            if (dictionary.TryGetValue(key, out var values))
                values.Add(value);
            else
                dictionary.Add(key, new() { value });
        }
    }
}
