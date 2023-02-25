namespace Arborescence
{
    using System.Collections.Generic;

    public readonly partial struct Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> :
        IReadOnlyDictionary<TKey, TValue>
        where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
        where TIndexToValueMap : IReadOnlyDictionary<TKey, int>
    {
        private readonly TKeyToIndexMap _indexByKey;
        private readonly TIndexToValueMap _valueByIndex;

        internal Int32IndirectReadOnlyDictionary(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
        {
            _indexByKey = indexByKey;
            _valueByIndex = valueByIndex;
        }

        public int Count => _valueByIndex.Count;

        public bool ContainsKey(TKey key) => throw new System.NotImplementedException();

        public bool TryGetValue(TKey key, out TValue value) => throw new System.NotImplementedException();

        public TValue this[TKey key] => throw new System.NotImplementedException();
    }
}
