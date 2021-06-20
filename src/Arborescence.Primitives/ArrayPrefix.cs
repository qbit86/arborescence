namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Primitives;

    /// <summary>
    /// A set of initialization methods for instances of <see cref="ArrayPrefix{T}"/>.
    /// </summary>
    public static class ArrayPrefix
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ArrayPrefix{T}"/> structure
        /// that delimits all the elements in the specified array.
        /// </summary>
        /// <param name="array">The array to wrap.</param>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <returns>An array prefix that delimits all the elements in the specified array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPrefix<T> Create<T>(T[] array)
        {
            return new ArrayPrefix<T>(array);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayPrefix{T}"/> structure
        /// that delimits the specified prefix in the specified array.
        /// </summary>
        /// <param name="array">The array which prefix to delimit.</param>
        /// <param name="count">The number of elements in the prefix.</param>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <returns>An array prefix that delimits <paramref name="count"/> elements in the specified array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPrefix<T> Create<T>(T[] array, int count)
        {
            return new ArrayPrefix<T>(array, count);
        }
    }

    // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/ArraySegment.cs

    /// <summary>
    /// Delimits a prefix of a one-dimensional array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public readonly struct ArrayPrefix<T> : IList<T>, IReadOnlyList<T>, IEquatable<ArrayPrefix<T>>
    {
#pragma warning disable CA1825 // Avoid zero-length array allocations.
        // Do not replace the array allocation with Array.Empty. We don't want to have the overhead of
        // instantiating another generic type in addition to ArrayPrefix<T> for new type parameters.
        /// <summary>
        /// Represents the empty array prefix.
        /// </summary>
        public static ArrayPrefix<T> Empty { get; } = new ArrayPrefix<T>(new T[0]);
#pragma warning restore CA1825 // Avoid zero-length array allocations.

        private readonly T[] _array;
        private readonly int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayPrefix{T}"/> structure
        /// that delimits all the elements in the specified array.
        /// </summary>
        /// <param name="array">The array to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        public ArrayPrefix(T[] array)
        {
            if (array is null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            _array = array;
            _count = array.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayPrefix{T}"/> structure
        /// that delimits the specified prefix in the specified array.
        /// </summary>
        /// <param name="array">The array which prefix to delimit.</param>
        /// <param name="count">The number of elements in the prefix.</param>
        public ArrayPrefix(T[] array, int count)
        {
            // Validate arguments, check is minimal instructions with reduced branching for inlinable fast-path
            // Negative values discovered though conversion to high values when converted to unsigned
            // Failure should be rare and location determination and message is delegated to failure functions
            if (array is null || (uint)count > (uint)array.Length)
                ArrayPrefixHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, 0, count);

            _array = array;
            _count = count;
        }

        /// <summary>
        /// Gets the original array containing the range of elements that the array prefix delimits.
        /// </summary>
        public T[] Array => _array;

        /// <summary>
        /// Gets the number of elements in the range delimited by the array prefix.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_count)
                    ArrayPrefixHelper.ThrowArgumentOutOfRangeException(nameof(index));

                return _array[index];
            }
            set
            {
                if ((uint)index >= (uint)_count)
                    ArrayPrefixHelper.ThrowArgumentOutOfRangeException(nameof(index));

                _array[index] = value;
            }
        }

        /// <summary>
        /// Returns an <see cref="ArrayPrefix{T}.Enumerator"/> that can be used to iterate through the array prefix.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public Enumerator GetEnumerator()
        {
            ThrowInvalidOperationIfDefault();
            return new Enumerator(_array, _count);
        }

        /// <summary>
        /// Returns an <see cref="ArrayPrefixEnumerator{T}"/> that can be used to iterate through the array prefix.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public ArrayPrefixEnumerator<T> Enumerate()
        {
            ThrowInvalidOperationIfDefault();
            return new ArrayPrefixEnumerator<T>(_array, _count);
        }

        /// <inheritdoc/>
        public override int GetHashCode() =>
            _array is null ? 0 : unchecked(_array.GetHashCode() * 397) ^ _count.GetHashCode();

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex = 0)
        {
            ThrowInvalidOperationIfDefault();
            System.Array.Copy(_array, 0, array, arrayIndex, _count);
        }

        /// <summary>
        /// Copies the contents of this instance into the specified destination array prefix
        /// of the same type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="destination">The array prefix into which the contents of this instance will be copied.</param>
        public void CopyTo(ArrayPrefix<T> destination)
        {
            ThrowInvalidOperationIfDefault();
            destination.ThrowInvalidOperationIfDefault();

            if (_count > destination._count)
                ArrayPrefixHelper.ThrowArgumentException_DestinationTooShort();

            System.Array.Copy(_array, 0, destination._array, 0, _count);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ArrayPrefix<T> other && Equals(other);

        /// <inheritdoc/>
        public bool Equals(ArrayPrefix<T> other) => other._array == _array && other._count == _count;

        /// <summary>
        /// Forms a slice out of the current array prefix starting at the specified index.
        /// </summary>
        /// <param name="index">The index at which to begin the slice.</param>
        /// <returns>
        /// An array segment that consists of all elements of the current array prefix
        /// from <paramref name="index"/> to the end of the array segment.
        /// </returns>
        public ArraySegment<T> Slice(int index)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count)
                ArrayPrefixHelper.ThrowArgumentOutOfRangeException(nameof(index));

            return new ArraySegment<T>(_array, index, _count - index);
        }

        /// <summary>
        /// Forms a slice of the specified length out of the current array prefix starting at the specified index.
        /// </summary>
        /// <param name="index">The index at which to begin the slice.</param>
        /// <param name="count">The desired length of the slice.</param>
        /// <returns>
        /// An array segment of <paramref name="count"/> elements starting at <paramref name="index"/>.
        /// </returns>
        public ArraySegment<T> Slice(int index, int count)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count || (uint)count > (uint)(_count - index))
                ArrayPrefixHelper.ThrowArgumentOutOfRangeException(nameof(index));

            return new ArraySegment<T>(_array, index, count);
        }

        /// <summary>
        /// Copies the contents of this array prefix into a new array.
        /// </summary>
        /// <returns>An array containing the elements in the current array prefix.</returns>
        public T[] ToArray()
        {
            ThrowInvalidOperationIfDefault();

            if (_count == 0)
                return Empty._array;

            var array = new T[_count];
            System.Array.Copy(_array, 0, array, 0, _count);
            return array;
        }

        /// <summary>
        /// Indicates whether two <see cref="ArrayPrefix{T}"/> structures are equal.
        /// </summary>
        /// <param name="a">The structure on the left side of the equality operator.</param>
        /// <param name="b">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="ArrayPrefix{T}"/> structures are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(ArrayPrefix<T> a, ArrayPrefix<T> b) => a.Equals(b);

        /// <summary>
        /// Indicates whether two <see cref="ArrayPrefix{T}"/> structures are not equal.
        /// </summary>
        /// <param name="a">The structure on the left side of the inequality operator.</param>
        /// <param name="b">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="ArrayPrefix{T}"/> structures are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(ArrayPrefix<T> a, ArrayPrefix<T> b) => !a.Equals(b);

        /// <summary>
        /// Defines an implicit conversion of an array to a <see cref="ArrayPrefix{T}"/>.
        /// </summary>
        /// <param name="array">The array to convert.</param>
        /// <returns>An array prefix.</returns>
        public static implicit operator ArrayPrefix<T>(T[] array) =>
            array != null ? new ArrayPrefix<T>(array) : default;

        #region IList<T>

        T IList<T>.this[int index]
        {
            get
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ArrayPrefixHelper.ThrowArgumentOutOfRangeException(nameof(index));

                return _array[index];
            }

            set
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ArrayPrefixHelper.ThrowArgumentOutOfRangeException(nameof(index));

                _array[index] = value;
            }
        }

        int IList<T>.IndexOf(T item)
        {
            ThrowInvalidOperationIfDefault();

            int index = System.Array.IndexOf(_array, item, 0, _count);

            Debug.Assert(index == -1 || index >= 0 && index < _count);

            return index >= 0 ? index : -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            ArrayPrefixHelper.ThrowNotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            ArrayPrefixHelper.ThrowNotSupportedException();
        }

        #endregion

        #region IReadOnlyList<T>

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ArrayPrefixHelper.ThrowArgumentOutOfRangeException(nameof(index));

                return _array[index];
            }
        }

        #endregion IReadOnlyList<T>

        #region ICollection<T>

        bool ICollection<T>.IsReadOnly => true;

        void ICollection<T>.Add(T item)
        {
            ArrayPrefixHelper.ThrowNotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            ArrayPrefixHelper.ThrowNotSupportedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            ThrowInvalidOperationIfDefault();

            int index = System.Array.IndexOf(_array, item, 0, _count);

            Debug.Assert(index == -1 || index >= 0 && index < _count);

            return index >= 0;
        }

        bool ICollection<T>.Remove(T item)
        {
            ArrayPrefixHelper.ThrowNotSupportedException();
            return default;
        }

        #endregion

        #region IEnumerable<T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Enumerate();

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => Enumerate();

        #endregion

        private void ThrowInvalidOperationIfDefault()
        {
            if (_array is null)
                ArrayPrefixHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NullArray);
        }

        /// <summary>
        /// Provides an enumerator for the elements of an <see cref="ArrayPrefix{T}"/>.
        /// </summary>
        public struct Enumerator
        {
            private readonly T[] _array;
            private readonly int _end;
            private int _current;

            internal Enumerator(T[] array, int count)
            {
                Debug.Assert(array != null);
                Debug.Assert(count >= 0);
                Debug.Assert(count <= array!.Length);

                _array = array;
                _end = count;
                _current = -1;
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                get
                {
                    if (_current < 0)
                        ArrayPrefixHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
                    if (_current >= _end)
                        ArrayPrefixHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
                    return _array[_current];
                }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the array prefix.
            /// </summary>
            /// <returns>
            /// <c>true</c> if the enumerator was successfully advanced to the next element;
            /// <c>false</c> if the enumerator has passed the end of the array prefix.
            /// </returns>
            public bool MoveNext()
            {
                if (_current < _end)
                {
                    _current++;
                    return _current < _end;
                }

                return false;
            }
        }
    }
}
