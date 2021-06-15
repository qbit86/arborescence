namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct IndexedDictionary<TKey, TValue, TIndexMap> :
        IReadOnlyDictionary<TKey, TValue>,
        IDictionary<TKey, TValue>,
        IEquatable<IndexedDictionary<TKey, TValue, TIndexMap>>
        where TIndexMap : IReadOnlyDictionary<TKey, int>
    {
        private readonly TValue[] _items;
        private readonly TIndexMap _indexMap;

        public IndexedDictionary(TValue[] items, TIndexMap indexMap)
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.items);

            if (indexMap == null)
                throw new ArgumentNullException(nameof(indexMap));

            _items = items;
            _indexMap = indexMap;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (!_indexMap.TryGetValue(item.Key, out int index))
                throw new ArgumentOutOfRangeException(nameof(item));

            if ((uint)index >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            _items[index] = item.Value;
        }

        public void Clear() => throw new NotSupportedException();

        public bool Contains(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotSupportedException();

        public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        public int Count => _items.Length;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (!_indexMap.TryGetValue(key, out int index))
                throw new ArgumentOutOfRangeException(nameof(key));

            if ((uint)index >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(key));

            _items[index] = value;
        }

        bool IDictionary<TKey, TValue>.ContainsKey(TKey key) => ContainsKey(key);

        public bool Remove(TKey key) => throw new NotSupportedException();

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValue(key, out value);

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) => ContainsKey(key);

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) =>
            TryGetValue(key, out value);

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public bool ContainsKey(TKey key)
        {
            if (!_indexMap.TryGetValue(key, out int index))
                return false;

            return unchecked((uint)index < (uint)_items.Length);
        }

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (!_indexMap.TryGetValue(key, out int index))
            {
                value = default;
                return false;
            }

            if (unchecked((uint)index >= (uint)_items.Length))
            {
                value = default;
                return false;
            }

            value = _items[index];
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!_indexMap.TryGetValue(key, out int index))
                    throw new KeyNotFoundException();

                if ((uint)index >= (uint)_items.Length)
                    throw new KeyNotFoundException();

                return _items[index];
            }
            set
            {
                if (!_indexMap.TryGetValue(key, out int index))
                    throw new ArgumentOutOfRangeException(nameof(key));

                if ((uint)index >= (uint)_items.Length)
                    throw new ArgumentOutOfRangeException(nameof(key));

                _items[index] = value;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw new NotSupportedException();

        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new NotSupportedException();

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw new NotSupportedException();

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        /// <summary>
        /// Gets an enumerable collection that contains the values in the dictionary.
        /// </summary>
        public IReadOnlyCollection<TValue> Values => _items;

        public bool Equals(IndexedDictionary<TKey, TValue, TIndexMap> other) =>
            Equals(_items, other._items) && EqualityComparer<TIndexMap>.Default.Equals(_indexMap, other._indexMap);

        public override bool Equals(object obj) =>
            obj is IndexedDictionary<TKey, TValue, TIndexMap> other && Equals(other);

        public override int GetHashCode()
        {
            return unchecked((_items != null ? _items.GetHashCode() : 0) * 397) ^
                EqualityComparer<TIndexMap>.Default.GetHashCode(_indexMap);
        }

        public static bool operator ==(IndexedDictionary<TKey, TValue, TIndexMap> left,
            IndexedDictionary<TKey, TValue, TIndexMap> right) => left.Equals(right);

        public static bool operator !=(IndexedDictionary<TKey, TValue, TIndexMap> left,
            IndexedDictionary<TKey, TValue, TIndexMap> right) => !left.Equals(right);
    }
}
