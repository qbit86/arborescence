namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    partial struct Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
    {
        public IEnumerable<TKey> Keys => throw new System.NotImplementedException();

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

        private IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorIterator() =>
            throw new System.NotImplementedException();
    }
}
