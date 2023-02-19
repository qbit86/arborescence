namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    partial struct Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy>
    {
        public IEnumerable<int> Keys
        {
            get
            {
                Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
                return self._items is null ? Enumerable.Empty<int>() : Enumerable.Range(0, self.CountUnchecked);
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                TValueList? items = _items;
                return items is null ? Enumerable.Empty<TValue>() : items;
            }
        }

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            return self._items is null
                ? Enumerable.Empty<KeyValuePair<int, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<int, TValue>> GetEnumeratorIterator()
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            int count = self.CountUnchecked;
            for (int key = 0; key < count; ++key)
            {
                TValue value = self._items[key];
                if (!_absencePolicy.Equals(value))
                    yield return new(key, value);
            }
        }

        public bool Equals(Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> other)
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            return EqualityComparer<TValueList>.Default.Equals(self._items, other._items) &&
                EqualityComparer<TAbsencePolicy>.Default.Equals(self._absencePolicy, other._absencePolicy);
        }

        public override bool Equals(object? obj) =>
            obj is Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> other && Equals(other);

        public override int GetHashCode()
        {
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> self = this;
            return HashCode.Combine(
                EqualityComparer<TValueList>.Default.GetHashCode(self._items),
                EqualityComparer<TAbsencePolicy>.Default.GetHashCode(self._absencePolicy));
        }

        public static bool operator ==(
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> right) => left.Equals(right);

        public static bool operator !=(
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> right) => !left.Equals(right);
    }
}
