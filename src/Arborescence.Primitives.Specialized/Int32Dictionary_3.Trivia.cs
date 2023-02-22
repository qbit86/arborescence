namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Primitives;

    public readonly partial struct Int32Dictionary<TValue, TValueList, TAbsenceComparer>
    {
        public bool IsReadOnly => false;

        IEnumerable<int> IReadOnlyDictionary<int, TValue>.Keys => throw new NotImplementedException();

        IEnumerable<TValue> IReadOnlyDictionary<int, TValue>.Values => throw new NotImplementedException();

        ICollection<int> IDictionary<int, TValue>.Keys => ThrowHelper.ThrowNotSupportedException<ICollection<int>>();

        ICollection<TValue> IDictionary<int, TValue>.Values =>
            ThrowHelper.ThrowNotSupportedException<ICollection<TValue>>();

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<int, TValue> item) => Add(item.Key, item.Value);

        public void Clear() => _items?.Clear();

        public bool Contains(KeyValuePair<int, TValue> item) => throw new NotImplementedException();

        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<int, TValue> item) => throw new NotImplementedException();

        public bool Remove(int key) => throw new NotImplementedException();

        bool IDictionary<int, TValue>.TryGetValue(int key, out TValue value) => TryGetValueCore(key, out value!);

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            TryGetValueCore(key, out value!);

        public bool Equals(Int32Dictionary<TValue, TValueList, TAbsenceComparer> other)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            return EqualityComparer<TValueList>.Default.Equals(self._items, other._items) &&
                EqualityComparer<TAbsenceComparer>.Default.Equals(self._absenceComparer, other._absenceComparer) &&
                EqualityComparer<TValue>.Default.Equals(self._absenceMarker!, other._absenceMarker!);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32Dictionary<TValue, TValueList, TAbsenceComparer> other && Equals(other);

        public override int GetHashCode()
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._items is not { } items)
                return 0;
            HashCode hashCode = new();
            if (self._absenceMarker is { } absenceMarker)
                hashCode.Add(EqualityComparer<TValue>.Default.GetHashCode(absenceMarker));
            hashCode.Add(EqualityComparer<TValueList>.Default.GetHashCode(items));
            hashCode.Add(EqualityComparer<TAbsenceComparer>.Default.GetHashCode(self._absenceComparer));
            return hashCode.ToHashCode();
        }

        public static bool operator ==(
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> left,
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> right) => left.Equals(right);

        public static bool operator !=(
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> left,
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> right) => !left.Equals(right);
    }
}