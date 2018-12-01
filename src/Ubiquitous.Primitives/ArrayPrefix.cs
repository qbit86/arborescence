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

        public ArrayPrefixEnumerator<T> GetEnumerator()
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

        public bool Equals(ArrayPrefix<T> obj)
        {
            return obj._array == _array && obj._count == _count;
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

        public static implicit operator ArrayPrefix<T>(T[] array)
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
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private void ThrowInvalidOperationIfDefault()
        {
            if (_array == null)
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NullArray);
        }
    }
}
