namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    partial struct Int32ReadOnlyDictionary<TValue, TValueList>
    {
        /// <inheritdoc/>
        public IEnumerable<int> Keys
        {
            get
            {
                int count = (_values?.Count).GetValueOrDefault();
                return count is 0 ? Enumerable.Empty<int>() : Enumerable.Range(0, count);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TValue> Values => _values ?? Enumerable.Empty<TValue>();

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            TryGetValueCore(key, out value!);

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            var self = this;
            return self._values is not { Count: > 0 }
                ? Enumerable.Empty<KeyValuePair<int, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<int, TValue>> GetEnumeratorIterator()
        {
            var values = _values;
            int count = values.Count;
            for (int key = 0; key < count; ++key)
                yield return new(key, values[key]);
        }

        /// <inheritdoc/>
        public bool Equals(Int32ReadOnlyDictionary<TValue, TValueList> other) =>
            EqualityComparer<TValueList>.Default.Equals(_values, other._values);

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32ReadOnlyDictionary<TValue, TValueList> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => EqualityComparer<TValueList>.Default.GetHashCode(_values);

        /// <summary>
        /// Indicates whether two
        /// <see cref="Int32ReadOnlyDictionary{TValue, TValueList}"/>
        /// structures are equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying lists are equal with respect to
        /// <see cref="EqualityComparer{TValueList}"/>.<see cref="EqualityComparer{TValueList}.Default"/>;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(
            Int32ReadOnlyDictionary<TValue, TValueList> left, Int32ReadOnlyDictionary<TValue, TValueList> right) =>
            left.Equals(right);

        /// <summary>
        /// Indicates whether two
        /// <see cref="Int32ReadOnlyDictionary{TValue, TValueList}"/>
        /// structures are not equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying lists are not equal with respect to
        /// <see cref="EqualityComparer{TValueList}"/>.<see cref="EqualityComparer{TValueList}.Default"/>;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(
            Int32ReadOnlyDictionary<TValue, TValueList> left, Int32ReadOnlyDictionary<TValue, TValueList> right) =>
            !left.Equals(right);
    }
}
