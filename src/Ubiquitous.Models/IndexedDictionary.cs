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
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private TValues _backingStore;

        internal TValues BackingStore => _backingStore;

        public IndexedDictionary(TValues backingStore)
        {
            if (backingStore == null)
                throw new ArgumentNullException(nameof(backingStore));

            _backingStore = backingStore;
        }

        public TValue this[int key]
        {
            get
            {
#if DEBUG
                if (key < 0)
                {
                    throw new IndexOutOfRangeException("Index was out of range. "
                        + $"Must be non-negative, but was {key}.");
                }

                if (key > _backingStore.Count)
                {
                    throw new IndexOutOfRangeException("Index was out of range. "
                        + $"Must be less than the size of the collection, but was {key}.");
                }
#endif

                return _backingStore[key];
            }
            set
            {
#if DEBUG
                if (key < 0)
                {
                    throw new IndexOutOfRangeException("Index was out of range. "
                        + $"Must be non-negative, but was {key}.");
                }

                if (key > _backingStore.Count)
                {
                    throw new IndexOutOfRangeException("Index was out of range. "
                        + $"Must be less than the size of the collection, but was {key}.");
                }
#endif

                _backingStore[key] = value;
            }
        }

        public ICollection<int> Keys => Enumerable.Range(0, _backingStore.Count).ToList();

        public ICollection<TValue> Values => _backingStore;

        public int Count => _backingStore.Count;

        public bool IsReadOnly => _backingStore.IsReadOnly;

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
            _backingStore.Clear();
        }

        public bool Contains(KeyValuePair<int, TValue> item)
        {
            throw new NotSupportedException();
        }

        public bool ContainsKey(int key)
        {
            if (key < 0)
                return false;

            if (key >= _backingStore.Count)
                return false;

            return true;
        }

        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            int count = _backingStore.Count;
            for (int i = 0; i != count; ++i)
            {
                TValue item = _backingStore[i];
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

            value = _backingStore[key];
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<KeyValuePair<int, TValue>> result = GetEnumerator();
            return result;
        }

        public bool Equals(IndexedDictionary<TValue, TValues> other)
        {
            if (!_backingStore.Equals(other._backingStore))
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

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            return _backingStore.GetHashCode();
        }
    }
}
