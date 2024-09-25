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
            var self = this;
            return self._indexByKey is null || self._valueByIndex is null
                ? Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorIterator()
        {
            var self = this;
            foreach ((var key, int index) in self._indexByKey)
            {
                if (self._valueByIndex.TryGetValue(index, out var value))
                    yield return new(key, value);
            }
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        /// <inheritdoc/>
        public void Clear() => _valueByIndex?.Clear();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item) =>
            TryGetValueCore(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(item.Value, value);

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array is null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                return;
            }

            if (unchecked((uint)arrayIndex > (uint)array.Length))
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(arrayIndex));

            var self = this;
            if (self._indexByKey is not { } indexByKey || self._valueByIndex is not { } valueByIndex)
                return;
            var destination = array.AsSpan(arrayIndex);
            int destinationLength = destination.Length;
            int destinationIndex = 0;
            foreach (var key in indexByKey.Keys)
            {
                if (!indexByKey.TryGetValue(key, out int index) || !valueByIndex.TryGetValue(index, out var value))
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
            var self = this;
            return self._indexByKey.TryGetValue(item.Key, out int index) &&
                self._valueByIndex.Remove(new KeyValuePair<int, TValue>(index, item.Value));
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            var self = this;
            return self._indexByKey.TryGetValue(key, out int index) && self._valueByIndex.Remove(index);
        }

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => TryGetValueCore(key, out value!);

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValueCore(key, out value!);

        /// <inheritdoc/>
        public bool Equals(Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other)
        {
            var self = this;
            return EqualityComparer<TKeyToIndexMap>.Default.Equals(self._indexByKey, other._indexByKey) &&
                EqualityComparer<TIndexToValueMap>.Default.Equals(self._valueByIndex, other._valueByIndex);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var self = this;
            return HashCode.Combine(
                EqualityComparer<TKeyToIndexMap>.Default.GetHashCode(self._indexByKey),
                EqualityComparer<TIndexToValueMap>.Default.GetHashCode(self._valueByIndex));
        }

        /// <summary>
        /// Indicates whether two <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// structures are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// structures are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> left,
            Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap> right) => !left.Equals(right);
    }
}
