namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Primitives;

    /// <summary>
    /// Represents a map from an index to a color as a byte array.
    /// </summary>
    public readonly struct Int32ColorDictionary :
        IReadOnlyDictionary<int, Color>, IDictionary<int, Color>, IEquatable<Int32ColorDictionary>
    {
        private readonly byte[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int32ColorDictionary"/> structure.
        /// </summary>
        /// <param name="items">The backing store for the map.</param>
        public Int32ColorDictionary(byte[] items) => _items = items;

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<int, Color>> GetEnumerator()
        {
            for (int i = 0; i < _items.Length; ++i)
                yield return new(i, (Color)_items[i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public void Add(KeyValuePair<int, Color> item)
        {
            if ((uint)item.Key >= (uint)_items.Length)
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(item));

            _items[item.Key] = (byte)item.Value;
        }

        /// <inheritdoc/>
        public void Clear() => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<int, Color> item)
        {
            if (unchecked((uint)item.Key >= (uint)_items.Length))
                return false;

            return _items[item.Key] == (byte)item.Value;
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<int, Color>[] array, int arrayIndex) =>
            ThrowHelper.ThrowNotSupportedException();

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
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(key));

            _items[key] = (byte)value;
        }

        /// <inheritdoc/>
        public bool Remove(int key) => throw new NotSupportedException();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public bool ContainsKey(int key) => unchecked((uint)key < (uint)_items.Length);

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public bool TryGetValue(int key, out Color value)
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
                    ArgumentOutOfRangeExceptionHelpers.Throw(nameof(key));

                _items[key] = (byte)value;
            }
        }

        ICollection<Color> IDictionary<int, Color>.Values => throw new NotSupportedException();

        ICollection<int> IDictionary<int, Color>.Keys => throw new NotSupportedException();

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public IEnumerable<int> Keys => Enumerable.Range(0, _items.Length);

        /// <inheritdoc cref="IReadOnlyDictionary{TKey,TValue}"/>
        public IEnumerable<Color> Values => _items.Cast<Color>();

        /// <inheritdoc/>
        public bool Equals(Int32ColorDictionary other) => Equals(_items, other._items);

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32ColorDictionary other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => _items.GetHashCode();

        /// <summary>
        /// Checks equality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(Int32ColorDictionary left, Int32ColorDictionary right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are not reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(Int32ColorDictionary left, Int32ColorDictionary right) =>
            !left.Equals(right);
    }
}
