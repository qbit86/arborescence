namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs
    public readonly struct ArrayPrefix<T> : IList<T>, IReadOnlyList<T>
    {
        // Do not replace the array allocation with Array.Empty. We don't want to have the overhead of
        // instantiating another generic type in addition to ArrayPrefix<T> for new type parameters.
        public static ArrayPrefix<T> Empty { get; } = new ArrayPrefix<T>(new T[0]);

        private const int _offset = 0;
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
            const int offset = 0;

            // Validate arguments, check is minimal instructions with reduced branching for inlinable fast-path
            // Negative values discovered though conversion to high values when converted to unsigned
            // Failure should be rare and location determination and message is delegated to failure functions
            if (array == null || (uint)count > (uint)(array.Length - offset))
                ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, offset, count);

            _array = array;
            _count = count;
        }

        public T[] Array => _array;

        public int Count => _count;

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_count)
                {
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();
                }

                return _array[_offset + index];
            }
            set
            {
                if ((uint)index >= (uint)_count)
                {
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();
                }

                _array[_offset + index] = value;
            }
        }

        public ArrayPrefixEnumerator<T> GetEnumerator()
        {
            ThrowInvalidOperationIfDefault();
            return new ArrayPrefixEnumerator<T>(_array, _count);
        }

        public override int GetHashCode()
        {
            if (_array == null)
            {
                return 0;
            }

            int hash = 5381;
            hash = HashHelpers.Combine(hash, _count);

            // The array hash is expected to be an evenly-distributed mixture of bits,
            // so rather than adding the cost of another rotation we just xor it.
            hash ^= _array.GetHashCode();
            return hash;
        }

        public void CopyTo(T[] destination) => CopyTo(destination, 0);

        public void CopyTo(T[] destination, int destinationIndex)
        {
            ThrowInvalidOperationIfDefault();
            System.Array.Copy(_array, _offset, destination, destinationIndex, _count);
        }

        public void CopyTo(ArrayPrefix<T> destination)
        {
            ThrowInvalidOperationIfDefault();
            destination.ThrowInvalidOperationIfDefault();

            if (_count > destination._count)
            {
                ThrowHelper.ThrowArgumentException_DestinationTooShort();
            }

            System.Array.Copy(_array, _offset, destination._array, _offset, _count);
        }

        public override bool Equals(object obj)
        {
            if (obj is ArrayPrefix<T> arrayPrefix)
                return Equals(arrayPrefix);

            return false;
        }

        public bool Equals(ArrayPrefix<T> obj)
        {
            return obj._array == _array && obj._count == _count;
        }

        public ArraySegment<T> Slice(int index)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count)
            {
                ThrowHelper.ThrowArgumentOutOfRange_IndexException();
            }

            return new ArraySegment<T>(_array, _offset + index, _count - index);
        }

        public ArraySegment<T> Slice(int index, int count)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count || (uint)count > (uint)(_count - index))
            {
                ThrowHelper.ThrowArgumentOutOfRange_IndexException();
            }

            return new ArraySegment<T>(_array, _offset + index, count);
        }

        public T[] ToArray()
        {
            ThrowInvalidOperationIfDefault();

            if (_count == 0)
            {
                return Empty._array;
            }

            var array = new T[_count];
            System.Array.Copy(_array, _offset, array, 0, _count);
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

        public static implicit operator ArrayPrefix<T>(T[] array) => array != null ? new ArrayPrefix<T>(array) : default;

        #region IList<T>

        T IList<T>.this[int index]
        {
            get
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();

                return _array[_offset + index];
            }

            set
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ThrowHelper.ThrowArgumentOutOfRange_IndexException();

                _array[_offset + index] = value;
            }
        }

        int IList<T>.IndexOf(T item)
        {
            ThrowInvalidOperationIfDefault();

            int index = System.Array.IndexOf(_array, item, _offset, _count);

            Debug.Assert(index == -1 ||
                            (index >= _offset && index < _offset + _count));

            return index >= 0 ? index - _offset : -1;
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

                return _array[_offset + index];
            }
        }

        #endregion IReadOnlyList<T>

        #region ICollection<T>

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                // the indexer setter does not throw an exception although IsReadOnly is true.
                // This is to match the behavior of arrays.
                return true;
            }
        }

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

            int index = System.Array.IndexOf(_array, item, _offset, _count);

            Debug.Assert(index == -1 ||
                            (index >= _offset && index < _offset + _count));

            return index >= 0;
        }

        bool ICollection<T>.Remove(T item)
        {
            ThrowHelper.ThrowNotSupportedException();
            return default;
        }

        #endregion

        #region IEnumerable<T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        private void ThrowInvalidOperationIfDefault()
        {
            if (_array == null)
            {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NullArray);
            }
        }
    }
}
