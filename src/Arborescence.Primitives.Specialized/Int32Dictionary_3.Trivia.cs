namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Primitives;

    public readonly partial struct Int32Dictionary<TValue, TValueList, TAbsencePolicy>
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

        public bool Equals(Int32Dictionary<TValue, TValueList, TAbsencePolicy> other) =>
            throw new NotImplementedException();

        public override bool Equals(object? obj) =>
            obj is Int32Dictionary<TValue, TValueList, TAbsencePolicy> other && Equals(other);

        public override int GetHashCode() => throw new NotImplementedException();

        public static bool operator ==(
            Int32Dictionary<TValue, TValueList, TAbsencePolicy> left,
            Int32Dictionary<TValue, TValueList, TAbsencePolicy> right) => left.Equals(right);

        public static bool operator !=(
            Int32Dictionary<TValue, TValueList, TAbsencePolicy> left,
            Int32Dictionary<TValue, TValueList, TAbsencePolicy> right) => !left.Equals(right);
    }
}
