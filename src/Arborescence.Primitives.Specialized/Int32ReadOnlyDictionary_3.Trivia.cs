namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    partial struct Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy>
    {
        public IEnumerable<int> Keys => throw new NotImplementedException();

        public IEnumerable<TValue> Values => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> other) =>
            _items.Equals(other._items) &&
            EqualityComparer<TAbsencePolicy>.Default.Equals(_absencePolicy, other._absencePolicy);

        public override bool Equals(object? obj) =>
            obj is Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(_items, _absencePolicy);

        public static bool operator ==(
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> right) => left.Equals(right);

        public static bool operator !=(
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> left,
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> right) => !left.Equals(right);
    }
}
