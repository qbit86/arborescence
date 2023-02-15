namespace Arborescence.Models
{
    using System.Collections.Generic;

    public readonly struct ListDictionaryMultimapPolicy<T> :
        IMultimapPolicy<T, Dictionary<T, List<T>>, List<T>.Enumerator>
    {
        public List<T>.Enumerator GetEnumerator(Dictionary<T, List<T>> multimap, T key) =>
            multimap.TryGetValue(key, out List<T>? values)
                ? values.GetEnumerator()
                : default(ListEnumerablePolicy<T>).GetEmptyEnumerator();
    }
}
