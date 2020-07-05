namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

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

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs

#pragma warning disable CA1710 // Identifiers should have correct suffix
    /// <summary>
    /// Delimits a prefix of a one-dimensional array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public readonly struct ArrayPrefix<T> : IList<T>, IReadOnlyList<T>, IEquatable<ArrayPrefix<T>>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA1825 // Avoid zero-length array allocations.
        // Do not replace the array allocation with Array.Empty. We don't want to have the overhead of
        // instantiating another generic type in addition to ArrayPrefix<T> for new type parameters.
        /// <summary>
        /// Represents the empty array prefix.
        /// </summary>
        public static ArrayPrefix<T> Empty { get; } = new ArrayPrefix<T>(new T[0]);
#pragma warning restore CA1825 // Avoid zero-length array allocations.
#pragma warning restore CA1000 // Do not declare static members on generic types

        private readonly T[] _array;
        private readonly int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayPrefix{T}"/> structure
        /// that delimits all the elements in the specified array.
        /// </summary>
        /// <param name="array">The array to wrap.</param>
        public ArrayPrefix(T[] array)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

#pragma warning disable CA1062
            _array = array;
            Debug.Assert(array != null, nameof(array) + " != null");
            _count = array.Length;
#pragma warning restore CA1062
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
            if (array == null || (uint)count > (uint)array.Length)
                ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, 0, count);

            _array = array;
            _count = count;
        }

#pragma warning disable CA1819 // Properties should not return arrays
        /// <summary>
        /// Gets the original array containing the range of elements that the array prefix delimits.
        /// </summary>
        public T[] Array => _array;
#pragma warning restore CA1819 // Properties should not return arrays

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
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();

                return _array[index];
            }
            set
            {
                if ((uint)index >= (uint)_count)
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();

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
        public override int GetHashCode()
        {
            if (_array == null)
                return 0;

            int hash = 5381;
            hash = HashHelpers.Combine(hash, _count);

            // The array hash is expected to be an evenly-distributed mixture of bits,
            // so rather than adding the cost of another rotation we just xor it.
            hash ^= _array.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Copies the contents of this instance into the specified destination array of the same type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="destination">
        /// The array of type <typeparamref name="T"/> into which the contents of this instance will be copied.
        /// </param>
        public void CopyTo(T[] destination)
        {
            CopyTo(destination, 0);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
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
                ThrowHelper.ThrowArgumentException_DestinationTooShort();

            System.Array.Copy(_array, 0, destination._array, 0, _count);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is ArrayPrefix<T> other && Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(ArrayPrefix<T> other)
        {
            return other._array == _array && other._count == _count;
        }

        public ArraySegment<T> Slice(int index)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count)
                ThrowHelper.ThrowArgumentOutOfRange_IndexException();

            return new ArraySegment<T>(_array, index, _count - index);
        }

        public ArraySegment<T> Slice(int index, int count)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count || (uint)count > (uint)(_count - index))
                ThrowHelper.ThrowArgumentOutOfRange_IndexException();

            return new ArraySegment<T>(_array, index, count);
        }

        public T[] ToArray()
        {
            ThrowInvalidOperationIfDefault();

            if (_count == 0)
                return Empty._array;

            var array = new T[_count];
            System.Array.Copy(_array, 0, array, 0, _count);
            return array;
        }

        public static bool operator ==(ArrayPrefix<T> a, ArrayPrefix<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ArrayPrefix<T> a, ArrayPrefix<T> b)
        {
            return !(a == b);
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        public static implicit operator ArrayPrefix<T>(T[] array)
        {
            return array != null ? new ArrayPrefix<T>(array) : default;
        }
#pragma warning restore CA2225 // Operator overloads have named alternates

        #region IList<T>

        T IList<T>.this[int index]
        {
            get
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();

                return _array[index];
            }

            set
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();

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
            ThrowHelper.ThrowNotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            ThrowHelper.ThrowNotSupportedException();
        }

        #endregion

        #region IReadOnlyList<T>

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();

                return _array[index];
            }
        }

        #endregion IReadOnlyList<T>

        #region ICollection<T>

        bool ICollection<T>.IsReadOnly => true;

        void ICollection<T>.Add(T item)
        {
            ThrowHelper.ThrowNotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            ThrowHelper.ThrowNotSupportedException();
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
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        #endregion

        #region IEnumerable<T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Enumerate();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerate();
        }

        #endregion

        private void ThrowInvalidOperationIfDefault()
        {
            if (_array == null)
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NullArray);
        }

#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable CA1034 // Nested types should not be visible
        public struct Enumerator
        {
            private readonly T[] _array;
            private readonly int _end;
            private int _current;

            internal Enumerator(T[] array, int count)
            {
                Debug.Assert(array != null);
                Debug.Assert(count >= 0);
                Debug.Assert(count <= array.Length);

                _array = array;
                _end = count;
                _current = -1;
            }

            public T Current
            {
                get
                {
                    if (_current < 0)
                        ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
                    if (_current >= _end)
                        ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
                    return _array[_current];
                }
            }

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
#pragma warning restore CA1034 // Nested types should not be visible
#pragma warning restore CA1815 // Override equals and operator equals on value types
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
