namespace Arborescence.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a map from an index to a color as a byte array.
    /// </summary>
    public readonly struct IndexedColorDictionary :
        IReadOnlyDictionary<int, Color>, IDictionary<int, Color>, IEquatable<IndexedColorDictionary>
    {
        private readonly byte[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedColorDictionary"/> structure.
        /// </summary>
        /// <param name="items">The backing store for the map.</param>
        public IndexedColorDictionary(byte[] items) => _items = items;

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<int, Color>> GetEnumerator()
        {
            for (int i = 0; i < _items.Length; ++i)
                yield return new KeyValuePair<int, Color>(i, (Color)_items[i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public void Add(KeyValuePair<int, Color> item)
        {
            if ((uint)item.Key >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            _items[item.Key] = (byte)item.Value;
        }

        /// <inheritdoc/>
        public void Clear() => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<int, Color> item)
        {
            if (unchecked((uint)item.Key >= (uint)_items.Length))
                return false;

            return _items[item.Key] == (byte)item.Value;
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<int, Color>[] array, int arrayIndex) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<int, Color> item) => throw new NotSupportedException();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public int Count => _items.Length;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(int key, Color value)
        {
            if ((uint)key >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(key));

            _items[key] = (byte)value;
        }

        bool IDictionary<int, Color>.ContainsKey(int key) => unchecked((uint)key < (uint)_items.Length);

        /// <inheritdoc/>
        public bool Remove(int key) => throw new NotSupportedException();

        bool IDictionary<int, Color>.TryGetValue(int key, out Color value)
        {
            if (unchecked((uint)key >= (uint)_items.Length))
            {
                value = default;
                return false;
            }

            value = (Color)_items[key];
            return true;
        }

        bool IReadOnlyDictionary<int, Color>.ContainsKey(int key) => unchecked((uint)key < (uint)_items.Length);

        bool IReadOnlyDictionary<int, Color>.TryGetValue(int key, out Color value)
        {
            if (unchecked((uint)key >= (uint)_items.Length))
            {
                value = default;
                return false;
            }

            value = (Color)_items[key];
            return true;
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public Color this[int key]
        {
            get
            {
                if ((uint)key >= (uint)_items.Length)
                    throw new KeyNotFoundException();

                return (Color)_items[key];
            }
            set
            {
                if ((uint)key >= (uint)_items.Length)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _items[key] = (byte)value;
            }
        }

        IEnumerable<int> IReadOnlyDictionary<int, Color>.Keys => Enumerable.Range(0, _items.Length);

        ICollection<Color> IDictionary<int, Color>.Values => throw new NotSupportedException();

        ICollection<int> IDictionary<int, Color>.Keys => throw new NotSupportedException();

        IEnumerable<Color> IReadOnlyDictionary<int, Color>.Values => _items.Cast<Color>();

        /// <inheritdoc/>
        public bool Equals(IndexedColorDictionary other) => Equals(_items, other._items);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is IndexedColorDictionary other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => _items != null ? _items.GetHashCode() : 0;

        /// <summary>
        /// Checks equality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(IndexedColorDictionary left, IndexedColorDictionary right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are not reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(IndexedColorDictionary left, IndexedColorDictionary right) => !left.Equals(right);
    }
}
