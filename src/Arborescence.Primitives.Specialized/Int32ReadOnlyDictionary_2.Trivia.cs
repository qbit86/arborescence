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
                Int32ReadOnlyDictionary<TValue, TValueList> self = this;
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

#if NETCOREAPP3_0_OR_GREATER
        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, [MaybeNullWhen(false)] out TValue value) =>
            TryGetValueCore(key, out value);
#else
        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            TryGetValueCore(key, out value!);
#endif

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
            int count = self.CountUnchecked;
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
