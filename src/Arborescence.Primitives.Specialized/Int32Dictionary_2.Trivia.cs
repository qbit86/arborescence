namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Primitives;

    partial struct Int32Dictionary<TValue, TValueList>
    {
        public bool IsReadOnly => false;

        public IEnumerable<int> Keys
        {
            get
            {
                int count = (_items?.Count).GetValueOrDefault();
                return count is 0 ? Enumerable.Empty<int>() : Enumerable.Range(0, count);
            }
        }

        public IEnumerable<TValue> Values => _items ?? Enumerable.Empty<TValue>();

        IEnumerable<int> IReadOnlyDictionary<int, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<int, TValue>.Values => Values;

        ICollection<int> IDictionary<int, TValue>.Keys => ThrowHelper.ThrowNotSupportedException<ICollection<int>>();

        ICollection<TValue> IDictionary<int, TValue>.Values => _items ?? (ICollection<TValue>)Array.Empty<TValue>();

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Clear() => _items?.Clear();

        public bool Contains(KeyValuePair<int, TValue> item) =>
            TryGetValueCore(item.Key, out TValue? value) && EqualityComparer<TValue>.Default.Equals(item.Value, value);

        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<int, TValue> item) => throw new NotImplementedException();

        public bool Remove(int key) => throw new NotImplementedException();

        bool IDictionary<int, TValue>.TryGetValue(int key, out TValue value) => throw new NotImplementedException();

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            throw new NotImplementedException();

        public bool Equals(Int32Dictionary<TValue, TValueList> other) =>
            EqualityComparer<TValueList>.Default.Equals(_items, other._items);

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32Dictionary<TValue, TValueList> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<TValueList>.Default.GetHashCode(_items);

        public static bool operator ==(
            Int32Dictionary<TValue, TValueList> left, Int32Dictionary<TValue, TValueList> right) => left.Equals(right);

        public static bool operator !=(
            Int32Dictionary<TValue, TValueList> left, Int32Dictionary<TValue, TValueList> right) => !left.Equals(right);
    }
}
