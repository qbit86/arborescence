namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
    {
        public bool IsReadOnly => false;

        public IEnumerable<TKey> Keys => throw new NotImplementedException();

        public IEnumerable<TValue> Values => throw new NotImplementedException();

        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new NotImplementedException();

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public bool Remove(TKey key) => throw new NotImplementedException();

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => TryGetValueCore(key, out value!);

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValueCore(key, out value!);

        public bool Equals(Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other)
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return EqualityComparer<TKeyToIndexMap>.Default.Equals(self._indexByKey, other._indexByKey) &&
                EqualityComparer<TIndexToValueMap>.Default.Equals(self._valueByIndex, other._valueByIndex);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other && Equals(other);

        public override int GetHashCode()
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return HashCode.Combine(
                EqualityComparer<TKeyToIndexMap>.Default.GetHashCode(self._indexByKey),
                EqualityComparer<TIndexToValueMap>.Default.GetHashCode(self._valueByIndex));
        }

        public static bool operator ==(
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) => left.Equals(right);

        public static bool operator !=(
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) => !left.Equals(right);
    }
}
