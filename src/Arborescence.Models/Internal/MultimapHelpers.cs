namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal static class MultimapHelpers<TValueCollection, TValueEnumerator>
    {
        internal static TValueEnumerator GetEnumerator<TKey, TMultimap, TEnumeratorProvider>(
            TMultimap multimap, TKey key, TEnumeratorProvider enumeratorProvider)
            where TMultimap : IDictionary<TKey, TValueCollection>
            where TEnumeratorProvider : IEnumeratorProvider<TValueCollection, TValueEnumerator> =>
            multimap.TryGetValue(key, out TValueCollection? values)
                ? enumeratorProvider.GetEnumerator(values)
                : enumeratorProvider.GetEmptyEnumerator();
    }
}
