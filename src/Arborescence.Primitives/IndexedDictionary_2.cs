namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Primitives;

    /// <summary>
    /// Represents a map from an index to a value, with a marker treated as an absent value.
    /// </summary>
    public readonly struct IndexedDictionary<TValue, TDummy> :
        IReadOnlyDictionary<int, TValue>, IDictionary<int, TValue>, IEquatable<IndexedDictionary<TValue, TDummy>>
        where TDummy : IEquatable<TValue>
    {
        private readonly TValue[] _items;
        private readonly TDummy _dummy;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedDictionary{TValue,TDummy}"/> structure.
        /// </summary>
        /// <param name="items">The backing store for the map.</param>
        /// <param name="dummy">The marker for missing values.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/> is <see langword="null"/>,
        /// or <paramref name="dummy"/> is <see langword="null"/>.
        /// </exception>
        public IndexedDictionary(TValue[] items, TDummy dummy)
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.items);

            if (dummy == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dummy);

            _items = items;
            _dummy = dummy;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            for (int i = 0; i < _items.Length; ++i)
                yield return new KeyValuePair<int, TValue>(i, _items[i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public void Add(KeyValuePair<int, TValue> item)
        {
            if ((uint)item.Key >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            _items[item.Key] = item.Value;
        }

        /// <inheritdoc/>
        public void Clear() => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<int, TValue> item) => throw new NotSupportedException();

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<int, TValue> item) => throw new NotSupportedException();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public int Count => _items.Length;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(int key, TValue value)
        {
            if ((uint)key >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(key));

            _items[key] = value;
        }

        bool IDictionary<int, TValue>.ContainsKey(int key) => ContainsKey(key);

        /// <inheritdoc/>
        public bool Remove(int key) => throw new NotSupportedException();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public bool ContainsKey(int key) => unchecked((uint)key < (uint)_items.Length) && !_dummy.Equals(_items[key]);

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value)
        {
            if (unchecked((uint)key >= (uint)_items.Length))
            {
                value = default;
                return false;
            }

            value = _items[key];
            return !_dummy.Equals(value);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public TValue this[int key]
        {
            get
            {
                if ((uint)key >= (uint)_items.Length)
                    throw new KeyNotFoundException();

                TValue result = _items[key];
                if (_dummy.Equals(result))
                    throw new KeyNotFoundException();

                return result;
            }
            set
            {
                if ((uint)key >= (uint)_items.Length)
                    throw new ArgumentOutOfRangeException(nameof(key));

                _items[key] = value;
            }
        }

        ICollection<TValue> IDictionary<int, TValue>.Values => throw new NotSupportedException();

        ICollection<int> IDictionary<int, TValue>.Keys => throw new NotSupportedException();

        IEnumerable<TValue> IReadOnlyDictionary<int, TValue>.Values => Values;

        /// <summary>
        /// Gets an enumerable collection that contains the keys in the dictionary.
        /// </summary>
        public IEnumerable<int> Keys
        {
            get
            {
                for (int i = 0; i < _items.Length; ++i)
                {
                    if (!_dummy.Equals(_items[i]))
                        yield return i;
                }
            }
        }

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
        public bool Equals(IndexedDictionary<TValue, TDummy> other) =>
            _dummy is TValue dummy && other._dummy.Equals(dummy) && Equals(_items, other._items);

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            obj is IndexedDictionary<TValue, TDummy> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() =>
            _items != null ? _items.GetHashCode() ^ _dummy.GetHashCode() : _dummy.GetHashCode();

        /// <summary>
        /// Checks equality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(
            IndexedDictionary<TValue, TDummy> left, IndexedDictionary<TValue, TDummy> right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are not reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(
            IndexedDictionary<TValue, TDummy> left, IndexedDictionary<TValue, TDummy> right) => !left.Equals(right);
    }
}
