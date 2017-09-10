namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class IndexedDictionary
    {
        public static IndexedDictionary<TValue, TValue[]> Create<TValue>(TValue[] backingStore)
        {
            return new IndexedDictionary<TValue, TValue[]>(backingStore);
        }
    }

    public struct IndexedDictionary<TValue, TValues> :
        IReadOnlyDictionary<int, TValue>,
        IDictionary<int, TValue>, IEquatable<IndexedDictionary<TValue, TValues>>

        where TValues : IList<TValue>
    {
        private TValues BackingStore { get; }

        public IndexedDictionary(TValues backingStore)
        {
            if (backingStore == null)
                throw new ArgumentNullException(nameof(backingStore));

            BackingStore = backingStore;
        }

        public TValue this[int key] { get => BackingStore[key]; set => BackingStore[key] = value; }

        public ICollection<int> Keys => Enumerable.Range(0, BackingStore.Count).ToList();

        public ICollection<TValue> Values => BackingStore;

        public int Count => BackingStore.Count;

        public bool IsReadOnly => BackingStore.IsReadOnly;

        IEnumerable<int> IReadOnlyDictionary<int, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<int, TValue>.Values => Values;

        public void Add(int key, TValue value)
        {
            throw new NotSupportedException();
        }

        public void Add(KeyValuePair<int, TValue> item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            BackingStore.Clear();
        }

        public bool Contains(KeyValuePair<int, TValue> item)
        {
            throw new NotSupportedException();
        }

        public bool ContainsKey(int key)
        {
            if (key < 0)
                return false;

            if (key >= BackingStore.Count)
                return false;

            return true;
        }

        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            int count = BackingStore.Count;
            for (int i = 0; i != count; ++i)
            {
                var item = BackingStore[i];
                yield return new KeyValuePair<int, TValue>(i, item);
            }
        }

        public bool Remove(int key)
        {
            throw new NotSupportedException();
        }

        public bool Remove(KeyValuePair<int, TValue> item)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(int key, out TValue value)
        {
            if (!ContainsKey(key))
            {
                value = default(TValue);
                return false;
            }

            value = BackingStore[key];
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<KeyValuePair<int, TValue>> result = GetEnumerator();
            return result;
        }

        public bool Equals(IndexedDictionary<TValue, TValues> other)
        {
            if (!BackingStore.Equals(other.BackingStore))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IndexedDictionary<TValue, TValues>))
                return false;

            var other = (IndexedDictionary<TValue, TValues>)obj;
            return Equals(other);
        }

        public override int GetHashCode() => BackingStore.GetHashCode();
    }
}
