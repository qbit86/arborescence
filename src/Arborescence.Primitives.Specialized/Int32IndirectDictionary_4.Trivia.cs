namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Primitives;

    partial struct Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
    {
        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public IEnumerable<TKey> Keys => _indexByKey?.Keys ?? Enumerable.Empty<TKey>();

        /// <inheritdoc/>
        public IEnumerable<TValue> Values => _valueByIndex?.Values ?? Enumerable.Empty<TValue>();

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                if (_indexByKey?.Keys is not { } keys)
                    return Array.Empty<TKey>();
                return keys as ICollection<TKey> ?? ThrowHelper.ThrowNotSupportedException<ICollection<TKey>>();
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values => _valueByIndex?.Values ?? Array.Empty<TValue>();

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return self._indexByKey is null || self._valueByIndex is null
                ? Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorIterator()
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            foreach ((TKey key, int index) in self._indexByKey)
            {
                if (self._valueByIndex.TryGetValue(index, out TValue? value))
                    yield return new(key, value);
            }
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        /// <inheritdoc/>
        public void Clear() => _valueByIndex?.Clear();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item) =>
            TryGetValueCore(item.Key, out TValue? value) && EqualityComparer<TValue>.Default.Equals(item.Value, value);

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array is null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                return;
            }

            if (unchecked((uint)arrayIndex > (uint)array.Length))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(arrayIndex));

            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            if (self._indexByKey is not { } indexByKey || self._valueByIndex is not { } valueByIndex)
                return;
            Span<KeyValuePair<TKey, TValue>> destination = array.AsSpan(arrayIndex);
            int destinationLength = destination.Length;
            int destinationIndex = 0;
            foreach (TKey key in indexByKey.Keys)
            {
                if (!indexByKey.TryGetValue(key, out int index) || !valueByIndex.TryGetValue(index, out TValue? value))
                    continue;
                if (destinationIndex < destinationLength)
                    destination[destinationIndex++] = new(key, value);
                else
                    ThrowHelper.ThrowDestinationArrayTooSmallException();
            }
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return self._indexByKey.TryGetValue(item.Key, out int index) &&
                self._valueByIndex.Remove(new KeyValuePair<int, TValue>(index, item.Value));
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return self._indexByKey.TryGetValue(key, out int index) && self._valueByIndex.Remove(index);
        }

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => TryGetValueCore(key, out value!);

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValueCore(key, out value!);

        /// <inheritdoc/>
        public bool Equals(Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other)
        {
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> self = this;
            return EqualityComparer<TKeyToIndexMap>.Default.Equals(self._indexByKey, other._indexByKey) &&
                EqualityComparer<TIndexToValueMap>.Default.Equals(self._valueByIndex, other._valueByIndex);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other && Equals(other);

        /// <inheritdoc/>
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
