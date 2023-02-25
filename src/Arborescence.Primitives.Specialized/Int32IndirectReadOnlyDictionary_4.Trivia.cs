namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    partial struct Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
    {
        public IEnumerable<TKey> Keys
        {
            get
            {
                Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
                if (self._indexByKey is not { } indexByKey)
                    return Enumerable.Empty<TKey>();
                return indexByKey.Keys
                    .Where(key => indexByKey.TryGetValue(key, out int index) && self._valueByIndex.ContainsKey(index));
            }
        }

        public IEnumerable<TValue> Values =>
            _valueByIndex is { } valueByIndex ? valueByIndex.Values : Enumerable.Empty<TValue>();

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValueCore(key, out value!);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return self._indexByKey is null
                ? Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorIterator()
        {
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            foreach (KeyValuePair<TKey, int> keyIndexPair in self._indexByKey)
            {
                int index = keyIndexPair.Value;
                if (self._valueByIndex.TryGetValue(index, out TValue? value))
                    yield return new(keyIndexPair.Key, value);
            }
        }
    }
}
