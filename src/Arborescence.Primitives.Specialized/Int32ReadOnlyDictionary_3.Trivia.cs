namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    partial struct Int32ReadOnlyDictionary<TValue, TValueList, TEquatable>
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
            {
                var value = values[key];
                if (!_absenceEquatable.Equals(value))
                    yield return new(key, value);
            }
        }

        /// <inheritdoc/>
        public bool Equals(Int32ReadOnlyDictionary<TValue, TValueList, TEquatable> other)
        {
            var self = this;
            return EqualityComparer<TValueList>.Default.Equals(self._values, other._values) &&
                EqualityComparer<TEquatable>.Default.Equals(self._absenceEquatable, other._absenceEquatable);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32ReadOnlyDictionary<TValue, TValueList, TEquatable> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var self = this;
            return HashCode.Combine(
                EqualityComparer<TValueList>.Default.GetHashCode(self._values),
                EqualityComparer<TEquatable>.Default.GetHashCode(self._absenceEquatable));
        }

        /// <summary>
        /// Indicates whether two
        /// <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TEquatable}"/>
        /// structures are equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying lists are equal, as well as absence objects;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(
            Int32ReadOnlyDictionary<TValue, TValueList, TEquatable> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TEquatable> right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two
        /// <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TEquatable}"/>
        /// structures are not equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying lists are not equal, as well as absence objects;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(
            Int32ReadOnlyDictionary<TValue, TValueList, TEquatable> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TEquatable> right) => !left.Equals(right);
    }
}
