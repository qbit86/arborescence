namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public readonly struct ArrayPrefix<T> : IList<T>, IReadOnlyList<T>, IEquatable<ArrayPrefix<T>>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
#pragma warning disable CA1825 // Avoid zero-length array allocations.
        // Do not replace the array allocation with Array.Empty. We don't want to have the overhead of
        // instantiating another generic type in addition to ArrayPrefix<T> for new type parameters.
        public static ArrayPrefix<T> Empty { get; } = new ArrayPrefix<T>(new T[0]);
#pragma warning restore CA1825 // Avoid zero-length array allocations.

        private readonly T[] _array;
        private readonly int _count;

        public ArrayPrefix(T[] array)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            _array = array;
            Debug.Assert(array != null, nameof(array) + " != null");
            _count = array.Length;
        }

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
        public T[] Array => _array;
#pragma warning restore CA1819 // Properties should not return arrays

        public int Count => _count;

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

        public Enumerator GetEnumerator()
        {
            ThrowInvalidOperationIfDefault();
            return new Enumerator(_array, _count);
        }

        public ArrayPrefixEnumerator<T> GetValueEnumerator()
        {
            ThrowInvalidOperationIfDefault();
            return new ArrayPrefixEnumerator<T>(_array, _count);
        }

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

        public void CopyTo(T[] destination)
        {
            CopyTo(destination, 0);
        }

        public void CopyTo(T[] destination, int destinationIndex)
        {
            ThrowInvalidOperationIfDefault();
            System.Array.Copy(_array, 0, destination, destinationIndex, _count);
        }

        public void CopyTo(ArrayPrefix<T> destination)
        {
            ThrowInvalidOperationIfDefault();
            destination.ThrowInvalidOperationIfDefault();

            if (_count > destination._count)
                ThrowHelper.ThrowArgumentException_DestinationTooShort();

            System.Array.Copy(_array, 0, destination._array, 0, _count);
        }

        public override bool Equals(object obj)
        {
            return obj is ArrayPrefix<T> other && Equals(other);
        }

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
#pragma warning restore CA2225 // Operator overloads have named alternates
        {
            return array != null ? new ArrayPrefix<T>(array) : default;
        }

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
            return GetValueEnumerator();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetValueEnumerator();
        }

        #endregion

        private void ThrowInvalidOperationIfDefault()
        {
            if (_array == null)
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NullArray);
        }

#pragma warning disable CA1815 // Override equals and operator equals on value types
        public struct Enumerator
#pragma warning restore CA1815 // Override equals and operator equals on value types
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
    }
}
