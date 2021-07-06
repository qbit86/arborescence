namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;

    /// <summary>
    /// Represents a set of values as a bit array.
    /// </summary>
#if NET5_0_OR_GREATER
    public readonly struct CompactSet : IReadOnlySet<int>, ISet<int>, IEquatable<CompactSet>
#else
    public readonly struct CompactSet : ISet<int>, IEquatable<CompactSet>
#endif
    {
        private const int BitShiftPerByte = 3;

        private readonly byte[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactSet"/> structure.
        /// </summary>
        /// <param name="items">The backing store for the set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
        public CompactSet(byte[] items)
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.items);

            _items = items;
        }

        /// <summary>
        /// Get the number of bytes required to hold <paramref name="count"/> bit values.
        /// </summary>
        /// <param name="count">The number of bit values.</param>
        /// <returns>The number of bytes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetByteCount(int count)
        {
            if (count <= 0)
                return 0;

            uint temp = (uint)(count - 1 + (1 << BitShiftPerByte));
            return (int)(temp >> BitShiftPerByte);
        }

        /// <inheritdoc/>
        public IEnumerator<int> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<int>.Add(int item)
        {
            int byteIndex = Div8Rem(item, out int bitIndex);
            if (unchecked((uint)byteIndex >= (uint)_items.Length))
                return;

            byte bitMask = (byte)(1u << bitIndex);
            _items[byteIndex] |= bitMask;
        }

        /// <inheritdoc/>
        public void ExceptWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc/>
        public void IntersectWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

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
        public void SymmetricExceptWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

        /// <inheritdoc/>
        public void UnionWith(IEnumerable<int> other) => ThrowHelper.ThrowNotSupportedException();

        bool ISet<int>.Add(int item) => Add(item);

        /// <inheritdoc cref="ISet{T}"/>
        public bool Add(int item)
        {
            int byteIndex = Div8Rem(item, out int bitIndex);
            if (unchecked((uint)byteIndex >= (uint)_items.Length))
                return false;

            byte bitMask = (byte)(1u << bitIndex);
            bool result = (_items[byteIndex] & bitMask) == 0;
            _items[byteIndex] |= bitMask;
            return result;
        }

        /// <summary>
        /// Removes all elements from a <see cref="CompactSet"/> object.
        /// </summary>
        public void Clear() => Array.Clear(_items, 0, _items.Length);

        /// <summary>
        /// Determines whether a <see cref="CompactSet"/> object contains the specified element.
        /// </summary>
        /// <param name="item">The element to locate in the <see cref="CompactSet"/> object.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="CompactSet"/> object contains the specified element;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(int item)
        {
            int byteIndex = Div8Rem(item, out int bitIndex);
            if (unchecked((uint)byteIndex >= (uint)_items.Length))
                return false;

            byte bitMask = (byte)(1u << bitIndex);
            return (_items[byteIndex] & bitMask) != 0;
        }

        /// <inheritdoc/>
        public void CopyTo(int[] array, int arrayIndex) => ThrowHelper.ThrowNotSupportedException();

        /// <summary>
        /// Removes the specified element from a <see cref="CompactSet"/> object.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the element is successfully found and removed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Remove(int item)
        {
            int byteIndex = Div8Rem(item, out int bitIndex);
            if (unchecked((uint)byteIndex >= (uint)_items.Length))
                return false;

            byte bitMask = unchecked((byte)~(1u << bitIndex));
            _items[byteIndex] &= bitMask;
            return true;
        }

        /// <inheritdoc/>
        public int Count => throw new NotSupportedException();

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public bool Equals(CompactSet other) => Equals(_items, other._items);

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is CompactSet other && Equals(other);

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
        public static bool operator ==(CompactSet left, CompactSet right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the underlying arrays are not reference equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(CompactSet left, CompactSet right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Div8Rem(int number, out int remainder)
        {
            uint quotient = (uint)number >> 3;
            remainder = number & 0b111;
            return (int)quotient;
        }
    }
}
