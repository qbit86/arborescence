namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32Dictionary<TValue, TValueList>
    {
        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Clear() => _items?.Clear();

        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<int, TValue> item) => throw new NotImplementedException();

        public bool Remove(int key) => throw new NotImplementedException();

        IEnumerable<int> IReadOnlyDictionary<int, TValue>.Keys => throw new NotImplementedException();

        ICollection<TValue> IDictionary<int, TValue>.Values => throw new NotImplementedException();

        ICollection<int> IDictionary<int, TValue>.Keys => throw new NotImplementedException();

        IEnumerable<TValue> IReadOnlyDictionary<int, TValue>.Values => throw new NotImplementedException();

        public bool Equals(Int32Dictionary<TValue, TValueList> other) =>
            EqualityComparer<TValueList>.Default.Equals(_items, other._items);

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32Dictionary<TValue, TValueList> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<TValueList>.Default.GetHashCode(_items);

        public static bool operator ==(
            Int32Dictionary<TValue, TValueList> left, Int32Dictionary<TValue, TValueList> right) => left.Equals(right);

        public static bool operator !=(
            Int32Dictionary<TValue, TValueList> left, Int32Dictionary<TValue, TValueList> right) => !left.Equals(right);

        public bool Contains(KeyValuePair<int, TValue> item) => throw new NotImplementedException();

        bool IReadOnlyDictionary<int, TValue>.ContainsKey(int key) => throw new NotImplementedException();

        bool IDictionary<int, TValue>.TryGetValue(int key, out TValue value) => throw new NotImplementedException();

        bool IDictionary<int, TValue>.ContainsKey(int key) => throw new NotImplementedException();

        bool IReadOnlyDictionary<int, TValue>.TryGetValue(int key, out TValue value) =>
            throw new NotImplementedException();
    }
}
