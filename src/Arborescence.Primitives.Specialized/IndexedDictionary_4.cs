namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Primitives;

    /// <summary>
    /// Represents an indirect key-to-value map via an intermediate index map, with a marker treated as an absent value.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TIndexMap">The type of the index map.</typeparam>
    /// <typeparam name="TDummy">The type of the marker for missing values.</typeparam>
    public readonly struct IndexedDictionary<TKey, TValue, TIndexMap, TDummy> :
        IReadOnlyDictionary<TKey, TValue>,
        IDictionary<TKey, TValue>,
        IEquatable<IndexedDictionary<TKey, TValue, TIndexMap, TDummy>>
        where TKey : notnull
        where TDummy : IEquatable<TValue>
        where TIndexMap : IReadOnlyDictionary<TKey, int>
    {
        private readonly TValue[] _items;
        private readonly TIndexMap _indexMap;
        private readonly TDummy _dummy;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedDictionary{TKey,TValue,TIndexMap,TDummy}"/> structure.
        /// </summary>
        /// <param name="items">The backing store for the map.</param>
        /// <param name="indexMap">The mapping from keys to indices in the backing store.</param>
        /// <param name="dummy">The marker for missing values.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/> is <see langword="null"/>,
        /// or <paramref name="indexMap"/> is <see langword="null"/>,
        /// or <paramref name="dummy"/> is <see langword="null"/>.
        /// </exception>
        public IndexedDictionary(TValue[] items, TIndexMap indexMap, TDummy dummy)
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.items);

            if (indexMap is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexMap));

            if (dummy is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dummy);

            _items = items;
            _indexMap = indexMap;
            _dummy = dummy;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (!_indexMap.TryGetValue(item.Key, out int index))
                throw new ArgumentOutOfRangeException(nameof(item));

            if ((uint)index >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            _items[index] = item.Value;
        }

        /// <inheritdoc/>
        public void Clear() => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public int Count => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            if (!_indexMap.TryGetValue(key, out int index))
                throw new ArgumentOutOfRangeException(nameof(key));

            if ((uint)index >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(key));

            _items[index] = value;
        }

        /// <inheritdoc/>
        public bool Remove(TKey key) => throw new NotSupportedException();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public bool ContainsKey(TKey key)
        {
            if (!_indexMap.TryGetValue(key, out int index))
                return false;

            return unchecked((uint)index < (uint)_items.Length) && !_dummy.Equals(_items[index]);
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
            return !_dummy.Equals(value);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public TValue this[TKey key]
        {
            get
            {
                if (!_indexMap.TryGetValue(key, out int index))
                    throw new KeyNotFoundException();

                if ((uint)index >= (uint)_items.Length)
                    throw new KeyNotFoundException();

                TValue result = _items[index];
                if (_dummy.Equals(result))
                    throw new KeyNotFoundException();

                return result;
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
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (TValue value in _items)
                {
                    if (!_dummy.Equals(value))
                        yield return value;
                }
            }
        }

        /// <inheritdoc/>
        public bool Equals(IndexedDictionary<TKey, TValue, TIndexMap, TDummy> other) =>
            EqualityComparer<TDummy>.Default.Equals(_dummy, other._dummy) &&
            Equals(_items, other._items) &&
            EqualityComparer<TIndexMap>.Default.Equals(_indexMap, other._indexMap);

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is IndexedDictionary<TKey, TValue, TIndexMap, TDummy> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (_items is null)
                return 0;
            return HashCode.Combine(
                EqualityComparer<TIndexMap>.Default.GetHashCode(_indexMap),
                EqualityComparer<TDummy>.Default.GetHashCode(_dummy),
                _items.GetHashCode());
        }

        /// <summary>
        /// Checks equality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(IndexedDictionary<TKey, TValue, TIndexMap, TDummy> left,
            IndexedDictionary<TKey, TValue, TIndexMap, TDummy> right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are not reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(IndexedDictionary<TKey, TValue, TIndexMap, TDummy> left,
            IndexedDictionary<TKey, TValue, TIndexMap, TDummy> right) => !left.Equals(right);
    }
}
