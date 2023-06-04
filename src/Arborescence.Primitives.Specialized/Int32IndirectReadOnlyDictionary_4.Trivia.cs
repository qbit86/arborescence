namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    partial struct Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IEnumerable<TValue> Values =>
            _valueByIndex is { } valueByIndex ? valueByIndex.Values : Enumerable.Empty<TValue>();

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValueCore(key, out value!);

        /// <inheritdoc/>
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
            foreach ((TKey key, int index) in self._indexByKey)
            {
                if (self._valueByIndex.TryGetValue(index, out TValue? value))
                    yield return new(key, value);
            }
        }

        /// <inheritdoc/>
        public bool Equals(Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other)
        {
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return EqualityComparer<TKeyToIndexMap>.Default.Equals(self._indexByKey, other._indexByKey) &&
                EqualityComparer<TIndexToValueMap>.Default.Equals(self._valueByIndex, other._valueByIndex);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other &&
            Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return HashCode.Combine(
                EqualityComparer<TKeyToIndexMap>.Default.GetHashCode(self._indexByKey),
                EqualityComparer<TIndexToValueMap>.Default.GetHashCode(self._valueByIndex));
        }

        public static bool operator ==(
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) =>
            left.Equals(right);

        public static bool operator !=(
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) =>
            !left.Equals(right);
    }
}
