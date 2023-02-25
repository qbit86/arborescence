namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;

    partial struct Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
    {
        public IEnumerable<TKey> Keys => throw new System.NotImplementedException();

        public IEnumerable<TValue> Values => throw new System.NotImplementedException();

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValueCore(key, out value!);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => throw new System.NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
