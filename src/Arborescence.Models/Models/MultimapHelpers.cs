namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal static class MultimapHelpers<TCollection, TEnumerator>
    {
        internal static TEnumerator Enumerate<TItem, TMultimap, TCollectionPolicy>(
            TMultimap multimap, TItem key, TCollectionPolicy policy)
            where TMultimap : IReadOnlyDictionary<TItem, TCollection>
            where TCollectionPolicy : IEnumerablePolicy<TCollection, TEnumerator> =>
            multimap.TryGetValue(key, out TCollection? values)
                ? policy.GetEnumerator(values)
                : policy.GetEmptyEnumerator();
    }
}
