namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Primitives;

    public readonly partial struct Int32Dictionary<TValue, TValueList, TAbsenceComparer>
    {
        int IReadOnlyCollection<KeyValuePair<int, TValue>>.Count => GetCount();

        int ICollection<KeyValuePair<int, TValue>>.Count => GetCount();

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        IEnumerable<int> IReadOnlyDictionary<int, TValue>.Keys
        {
            get
            {
                Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
                if (self._values is not { Count: > 0 } values)
                    return Enumerable.Empty<int>();
                return values.Select((value, index) => new KeyValuePair<int, TValue>(index, value))
                    .Where(it => !self.IsAbsence(it.Value)).Select(it => it.Key);
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<int, TValue>.Values
        {
            get
            {
                Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
                return self._values is not { Count: > 0 } values
                    ? Enumerable.Empty<TValue>()
                    : values.Where(value => !self.IsAbsence(value));
            }
        }

        ICollection<int> IDictionary<int, TValue>.Keys => _values is not { Count: > 0 }
            ? Array.Empty<int>()
            : ThrowHelper.ThrowNotSupportedException<ICollection<int>>();

        ICollection<TValue> IDictionary<int, TValue>.Values =>
            ThrowHelper.ThrowNotSupportedException<ICollection<TValue>>();

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            return self._values is not { Count: > 0 }
                ? Enumerable.Empty<KeyValuePair<int, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<int, TValue>> GetEnumeratorIterator()
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            TValueList values = self._values;
            int count = values.Count;
            for (int key = 0; key < count; ++key)
            {
                TValue value = values[key];
                if (!self.IsAbsence(value))
                    yield return new(key, values[key]);
            }
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<int, TValue> item) => Add(item.Key, item.Value);

        /// <inheritdoc/>
        public void Clear() => _values?.Clear();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<int, TValue> item) =>
            TryGetValueCore(item.Key, out TValue? value) && EqualityComparer<TValue>.Default.Equals(item.Value, value);

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex)
        {
            if (array is null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                return;
            }

            if (unchecked((uint)arrayIndex > (uint)array.Length))
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(arrayIndex));

            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._values is not { } values)
                return;
            Span<KeyValuePair<int, TValue>> destination = array.AsSpan(arrayIndex);
            int sourceCount = values.Count;
            int destinationLength = destination.Length;
            for (int sourceIndex = 0, destinationIndex = 0; sourceIndex < sourceCount; ++sourceIndex)
            {
                TValue value = values[sourceIndex];
                if (IsAbsence(value))
                    continue;
                if (destinationIndex < destinationLength)
                    destination[destinationIndex++] = new(sourceIndex, value);
                else
                    ThrowHelper.ThrowDestinationArrayTooSmallException();
            }
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<int, TValue> item)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (_values is not { } values)
                return false;
            int key = item.Key;
            int count = values.Count;
            if (unchecked((uint)key >= (uint)count))
                return false;
            TValue existingValue = values[key];
            if (self._absenceComparer.Equals(existingValue, self._absenceMarker!) ||
                !EqualityComparer<TValue>.Default.Equals(existingValue, item.Value))
                return false;
            values[key] = self._absenceMarker!;
            return true;
        }

        /// <inheritdoc/>
        public bool Remove(int key)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._values is not { } values)
                return false;
            int count = values.Count;
            if (unchecked((uint)key >= (uint)count))
                return false;
            values[key] = self._absenceMarker!;
            return true;
        }

        bool IDictionary<int, TValue>.TryGetValue(int key, out TValue value) => TryGetValueCore(key, out value!);

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            TryGetValueCore(key, out value!);

        /// <inheritdoc/>
        public bool Equals(Int32Dictionary<TValue, TValueList, TAbsenceComparer> other)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            return EqualityComparer<TValueList>.Default.Equals(self._values, other._values) &&
                EqualityComparer<TAbsenceComparer>.Default.Equals(self._absenceComparer, other._absenceComparer) &&
                EqualityComparer<TValue>.Default.Equals(self._absenceMarker!, other._absenceMarker!);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32Dictionary<TValue, TValueList, TAbsenceComparer> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            return HashCode.Combine(EqualityComparer<TValueList>.Default.GetHashCode(self._values),
                EqualityComparer<TAbsenceComparer>.Default.GetHashCode(self._absenceComparer),
                EqualityComparer<TValue>.Default.GetHashCode(self._absenceMarker!));
        }

        /// <summary>
        /// Indicates whether two <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>
        /// structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>
        /// structures are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> left,
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>
        /// structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>
        /// structures are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> left,
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> right) => !left.Equals(right);
    }
}
