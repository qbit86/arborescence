namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    partial struct Int32ReadOnlyDictionary<TValue, TValueList, TAbsence>
    {
        public IEnumerable<int> Keys
        {
            get
            {
                int count = (_values?.Count).GetValueOrDefault();
                return count is 0 ? Enumerable.Empty<int>() : Enumerable.Range(0, count);
            }
        }

        public IEnumerable<TValue> Values => _values ?? Enumerable.Empty<TValue>();

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            TryGetValueCore(key, out value!);

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> self = this;
            return self._values is not { Count: > 0 }
                ? Enumerable.Empty<KeyValuePair<int, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<int, TValue>> GetEnumeratorIterator()
        {
            TValueList values = _values;
            int count = values.Count;
            for (int key = 0; key < count; ++key)
            {
                TValue value = values[key];
                if (!_absence.Equals(value))
                    yield return new(key, value);
            }
        }

        public bool Equals(Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> other)
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> self = this;
            return EqualityComparer<TValueList>.Default.Equals(self._values, other._values) &&
                EqualityComparer<TAbsence>.Default.Equals(self._absence, other._absence);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> other && Equals(other);

        public override int GetHashCode()
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> self = this;
            return HashCode.Combine(
                EqualityComparer<TValueList>.Default.GetHashCode(self._values),
                EqualityComparer<TAbsence>.Default.GetHashCode(self._absence));
        }

        public static bool operator ==(
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> right) => left.Equals(right);

        public static bool operator !=(
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> right) => !left.Equals(right);
    }
}
