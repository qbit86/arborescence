namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal readonly struct ListMultimapConcept<TMultimap, TKey, TValue> :
        IReadOnlyMultimapConcept<TMultimap, TKey, List<TValue>.Enumerator>,
        IDictionaryAddition<TMultimap, TKey, TValue>
        where TMultimap : IDictionary<TKey, List<TValue>>
    {
        private static ListEnumeratorProvider<TValue> EnumeratorProvider => default;

        public List<TValue>.Enumerator EnumerateValues(TMultimap multimap, TKey key) =>
            MultimapHelpers<List<TValue>, List<TValue>.Enumerator>.GetEnumerator(multimap, key, EnumeratorProvider);

        public int GetCount(TMultimap multimap) => multimap.Count;

        public void Add(TMultimap dictionary, TKey key, TValue value)
        {
            if (dictionary.TryGetValue(key, out List<TValue>? values))
                values.Add(value);
            else
                dictionary.Add(key, new() { value });
        }
    }
}
