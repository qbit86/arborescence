namespace Arborescence
{
    using System;
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

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
