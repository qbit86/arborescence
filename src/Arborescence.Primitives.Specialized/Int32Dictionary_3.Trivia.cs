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

        public bool IsReadOnly => false;

        IEnumerable<int> IReadOnlyDictionary<int, TValue>.Keys
        {
            get
            {
                Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
                if (self._items is not { Count: > 0 } items)
                    return Enumerable.Empty<int>();
                return items.Select((value, index) => new KeyValuePair<int, TValue>(index, value))
                    .Where(it => !self.IsAbsence(it.Value)).Select(it => it.Key);
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<int, TValue>.Values
        {
            get
            {
                Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
                return self._items is not { Count: > 0 } items
                    ? Enumerable.Empty<TValue>()
                    : items.Where(value => !self.IsAbsence(value));
            }
        }

        ICollection<int> IDictionary<int, TValue>.Keys => _items is not { Count: > 0 }
            ? Array.Empty<int>()
            : ThrowHelper.ThrowNotSupportedException<ICollection<int>>();

        ICollection<TValue> IDictionary<int, TValue>.Values =>
            ThrowHelper.ThrowNotSupportedException<ICollection<TValue>>();

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            return self._items is not { Count: > 0 }
                ? Enumerable.Empty<KeyValuePair<int, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<int, TValue>> GetEnumeratorIterator()
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            TValueList items = self._items;
            int count = items.Count;
            for (int key = 0; key < count; ++key)
            {
                TValue value = items[key];
                if (!self.IsAbsence(value))
                    yield return new(key, items[key]);
            }
        }

        public void Add(KeyValuePair<int, TValue> item) => Add(item.Key, item.Value);

        public void Clear() => _items?.Clear();

        public bool Contains(KeyValuePair<int, TValue> item) =>
            TryGetValueCore(item.Key, out TValue? value) && EqualityComparer<TValue>.Default.Equals(item.Value, value);

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
            if (self._items is not { } items)
                return;
            Span<KeyValuePair<int, TValue>> destination = array.AsSpan(arrayIndex);
            int sourceCount = items.Count;
            int destinationLength = destination.Length;
            for (int sourceIndex = 0, destinationIndex = 0; sourceIndex < sourceCount; ++sourceIndex)
            {
                TValue value = items[sourceIndex];
                if (IsAbsence(value))
                    continue;
                if (destinationIndex < destinationLength)
                    destination[destinationIndex++] = new(sourceIndex, value);
                else
                    ThrowHelper.ThrowDestinationArrayTooSmallException();
            }
        }

        public bool Remove(KeyValuePair<int, TValue> item)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (_items is not { } items)
                return false;
            int key = item.Key;
            int count = items.Count;
            if (unchecked((uint)key >= (uint)count))
                return false;
            TValue existingValue = items[key];
            if (self._absenceComparer.Equals(existingValue, self._absenceMarker) ||
                !EqualityComparer<TValue>.Default.Equals(existingValue, item.Value))
                return false;
            items[key] = self._absenceMarker!;
            return true;
        }

        public bool Remove(int key)
        {
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> self = this;
            if (self._items is not { } items)
                return false;
            int count = items.Count;
            if (unchecked((uint)key >= (uint)count))
                return false;
            items[key] = self._absenceMarker!;
            return true;
        }

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
            HashCode hashCode = new();
            if (self._items is { Count: > 0 } items)
                hashCode.Add(EqualityComparer<TValueList>.Default.GetHashCode(items));
            hashCode.Add(EqualityComparer<TAbsenceComparer>.Default.GetHashCode(self._absenceComparer));
            if (self._absenceMarker is { } absenceMarker)
                hashCode.Add(EqualityComparer<TValue>.Default.GetHashCode(absenceMarker));
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
