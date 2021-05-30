namespace Arborescence.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a set of values as a byte array.
    /// </summary>
#if NET5
    public readonly struct IndexedSet : IReadOnlySet<int>, ISet<int>, IEquatable<IndexedSet>
#else
    public readonly struct IndexedSet : ISet<int>, IEquatable<IndexedSet>
#endif
    {
        private readonly byte[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedSet"/> structure.
        /// </summary>
        /// <param name="items">The backing store for the set.</param>
        public IndexedSet(byte[] items) => _items = items;

        /// <inheritdoc/>
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _items.Length; ++i)
            {
                if (_items[i] != 0)
                    yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<int>.Add(int item)
        {
            if ((uint)item >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            _items[item] = 1;
        }

        /// <inheritdoc/>
        public void ExceptWith(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public void IntersectWith(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsProperSubsetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsProperSupersetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsSubsetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsSupersetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool Overlaps(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool SetEquals(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public void SymmetricExceptWith(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public void UnionWith(IEnumerable<int> other) => throw new NotSupportedException();

        bool ISet<int>.Add(int item)
        {
            if ((uint)item >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            bool result = _items[item] == 0;
            _items[item] = 1;
            return result;
        }

        /// <summary>
        /// Removes all elements from a <see cref="IndexedSet"/> object.
        /// </summary>
        public void Clear() => Array.Clear(_items, 0, _items.Length);

        /// <summary>
        /// Determines whether a <see cref="IndexedSet"/> object contains the specified element.
        /// </summary>
        /// <param name="item">The element to locate in the <see cref="IndexedSet"/> object.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="IndexedSet"/> object contains the specified element;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(int item)
        {
            if (unchecked((uint)item >= (uint)_items.Length))
                return false;

            return _items[item] != 0;
        }

        /// <inheritdoc/>
        public void CopyTo(int[] array, int arrayIndex) => throw new NotSupportedException();

        /// <summary>
        /// Removes the specified element from a <see cref="IndexedSet"/> object.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the element is successfully found and removed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Remove(int item)
        {
            if (unchecked((uint)item >= (uint)_items.Length))
                return false;

            _items[item] = 0;
            return true;
        }

        /// <inheritdoc/>
        public int Count => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public bool Equals(IndexedSet other) => Equals(_items, other._items);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is IndexedSet other && Equals(other);

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
        public static bool operator ==(IndexedSet left, IndexedSet right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are not reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(IndexedSet left, IndexedSet right) => !left.Equals(right);
    }
}
