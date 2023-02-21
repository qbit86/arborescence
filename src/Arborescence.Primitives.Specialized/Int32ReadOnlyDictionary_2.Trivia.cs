namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    partial struct Int32ReadOnlyDictionary<TValue, TValueList>
    {
        public IEnumerable<int> Keys
        {
            get
            {
                int count = (_items?.Count).GetValueOrDefault();
                return count is 0 ? Enumerable.Empty<int>() : Enumerable.Range(0, count);
            }
        }

        public IEnumerable<TValue> Values => _items ?? Enumerable.Empty<TValue>();

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            TryGetValueCore(key, out value!);

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            Int32ReadOnlyDictionary<TValue, TValueList> self = this;
            return self._items is null
                ? Enumerable.Empty<KeyValuePair<int, TValue>>().GetEnumerator()
                : self.GetEnumeratorIterator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<int, TValue>> GetEnumeratorIterator()
        {
            Int32ReadOnlyDictionary<TValue, TValueList> self = this;
            int count = self._items.Count;
            for (int key = 0; key < count; ++key)
                yield return new(key, self._items[key]);
        }

        public bool Equals(Int32ReadOnlyDictionary<TValue, TValueList> other) =>
            EqualityComparer<TValueList>.Default.Equals(_items, other._items);

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32ReadOnlyDictionary<TValue, TValueList> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<TValueList>.Default.GetHashCode(_items);

        public static bool operator ==(
            Int32ReadOnlyDictionary<TValue, TValueList> left, Int32ReadOnlyDictionary<TValue, TValueList> right) =>
            left.Equals(right);

        public static bool operator !=(
            Int32ReadOnlyDictionary<TValue, TValueList> left, Int32ReadOnlyDictionary<TValue, TValueList> right) =>
            !left.Equals(right);
    }
}
