namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Primitives;

    /// <summary>
    /// Represents a set of values as a byte array.
    /// </summary>
    public readonly struct Int32Set : ISet<int>,
#if NET5_0_OR_GREATER
        IReadOnlySet<int>,
#endif
        IEquatable<Int32Set>
    {
        private readonly byte[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int32Set"/> structure.
        /// </summary>
        /// <param name="items">The backing store for the set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
        public Int32Set(byte[] items)
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.items);

            _items = items;
        }

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
        public void ExceptWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc/>
        public void IntersectWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc cref="ISet{T}.IsProperSubsetOf"/>
        public bool IsProperSubsetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc cref="ISet{T}.IsProperSupersetOf"/>
        public bool IsProperSupersetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc cref="ISet{T}.IsSubsetOf"/>
        public bool IsSubsetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc cref="ISet{T}.IsSupersetOf"/>
        public bool IsSupersetOf(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc cref="ISet{T}.Overlaps"/>
        public bool Overlaps(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc cref="ISet{T}.SetEquals"/>
        public bool SetEquals(IEnumerable<int> other) => throw new NotSupportedException();

        /// <inheritdoc/>
        public void SymmetricExceptWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc/>
        public void UnionWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc cref="ISet{T}"/>
        public bool Add(int item)
        {
            if ((uint)item >= (uint)_items.Length)
                throw new ArgumentOutOfRangeException(nameof(item));

            bool result = _items[item] == 0;
            _items[item] = 1;
            return result;
        }

        /// <summary>
        /// Removes all elements from a <see cref="Int32Set"/> object.
        /// </summary>
        public void Clear() => Array.Clear(_items, 0, _items.Length);

        /// <summary>
        /// Determines whether a <see cref="Int32Set"/> object contains the specified element.
        /// </summary>
        /// <param name="item">The element to locate in the <see cref="Int32Set"/> object.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Int32Set"/> object contains the specified element;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(int item)
        {
            if (unchecked((uint)item >= (uint)_items.Length))
                return false;

            return _items[item] != 0;
        }

        /// <inheritdoc/>
        public void CopyTo(int[] array, int arrayIndex) => ThrowHelper.ThrowNotSupportedException();

        /// <summary>
        /// Removes the specified element from a <see cref="Int32Set"/> object.
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

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public bool Equals(Int32Set other) => Equals(_items, other._items);

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32Set other && Equals(other);

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
        public static bool operator ==(Int32Set left, Int32Set right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are not reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(Int32Set left, Int32Set right) => !left.Equals(right);
    }
}
