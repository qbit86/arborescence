namespace Arborescence
{
    using System.Collections.Generic;
    using Primitives;

    public static class Int32IndirectDictionaryFactory<TKey, TValue>
    {
        public static Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
            Create<TKeyToIndexMap, TIndexToValueMap>(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TIndexToValueMap : IDictionary<int, TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (valueByIndex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(valueByIndex));
            return new(indexByKey, valueByIndex);
        }
    }
}
