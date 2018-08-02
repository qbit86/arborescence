namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public readonly struct ReadOnlyArraySegment<T> : IReadOnlyList<T>
    {
        public static ReadOnlyArraySegment<T> Empty { get; } = new ReadOnlyArraySegment<T>(new T[0]);

        private readonly T[] _array;
        private readonly int _offset;
        private readonly int _count;

        public ReadOnlyArraySegment(T[] array)
        {
            _array = array ?? throw new ArgumentNullException(nameof(array));
            _offset = 0;
            _count = array.Length;
        }

        public ReadOnlyArraySegment(T[] array, int offset, int count)
        {
            if (array == null || (uint)offset > (uint)array.Length || (uint)count > (uint)(array.Length - offset))
                ThrowArraySegmentCtorValidationFailedExceptions(array, offset, count);

            _array = array;
            _offset = offset;
            _count = count;
        }

        public T[] Array => _array;

        public int Offset => _offset;

        public int Count => _count;

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_count)
                {
                    ThrowArgumentOutOfRange_IndexException();
                }

                return _array[_offset + index];
            }
            set
            {
                if ((uint)index >= (uint)_count)
                {
                    ThrowArgumentOutOfRange_IndexException();
                }

                _array[_offset + index] = value;
            }
        }

        public Enumerator GetEnumerator()
        {
            ThrowInvalidOperationIfDefault();
            return new Enumerator(this);
        }

        public override int GetHashCode()
        {
            if (_array == null)
            {
                return 0;
            }

            int hash = 5381;
            hash = Combine(hash, _offset);
            hash = Combine(hash, _count);

            hash ^= _array.GetHashCode();
            return hash;
        }

        public void CopyTo(T[] destination) => CopyTo(destination, 0);

        public void CopyTo(T[] destination, int destinationIndex)
        {
            ThrowInvalidOperationIfDefault();
            System.Array.Copy(_array, _offset, destination, destinationIndex, _count);
        }

        public void CopyTo(ArraySegment<T> destination)
        {
            ThrowInvalidOperationIfDefault();

            if (destination.Array == null)
                throw new InvalidOperationException("The underlying array is null.");

            if (_count > destination.Count)
            {
                ThrowArgumentException_DestinationTooShort();
            }

            System.Array.Copy(_array, _offset, destination.Array, destination.Offset, _count);
        }

        public override bool Equals(Object obj)
        {
            if (obj is ReadOnlyArraySegment<T> other)
                return Equals(other);
            else
                return false;
        }

        public bool Equals(ReadOnlyArraySegment<T> obj)
        {
            return obj._array == _array && obj._offset == _offset && obj._count == _count;
        }

        public ReadOnlyArraySegment<T> Slice(int index)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count)
            {
                ThrowArgumentOutOfRange_IndexException();
            }

            return new ReadOnlyArraySegment<T>(_array, _offset + index, _count - index);
        }

        public ReadOnlyArraySegment<T> Slice(int index, int count)
        {
            ThrowInvalidOperationIfDefault();

            if ((uint)index > (uint)_count || (uint)count > (uint)(_count - index))
            {
                ThrowArgumentOutOfRange_IndexException();
            }

            return new ReadOnlyArraySegment<T>(_array, _offset + index, count);
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

        public static bool operator ==(ReadOnlyArraySegment<T> a, ReadOnlyArraySegment<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ReadOnlyArraySegment<T> a, ReadOnlyArraySegment<T> b)
        {
            return !(a == b);
        }

        public static implicit operator ReadOnlyArraySegment<T>(T[] array) => array != null ? new ReadOnlyArraySegment<T>(array) : default;

        #region IReadOnlyList<T>
        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                ThrowInvalidOperationIfDefault();
                if (index < 0 || index >= _count)
                    ThrowArgumentOutOfRange_IndexException();

                return _array[_offset + index];
            }
        }
        #endregion IReadOnlyList<T>

        #region IEnumerable<T>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        private static int Combine(int h1, int h2)
        {
            unchecked
            {
                uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
                return ((int)rol5 + h1) ^ h2;
            }
        }

        private static void ThrowArraySegmentCtorValidationFailedExceptions(Array array, int offset, int count)
        {
            throw GetArraySegmentCtorValidationFailedException(array, offset, count);
        }

        private static Exception GetArraySegmentCtorValidationFailedException(Array array, int offset, int count)
        {
            if (array == null)
                return new ArgumentNullException(nameof(array));

            if (offset < 0)
                return new ArgumentOutOfRangeException(nameof(offset), "Non-negative number required.");

            if (count < 0)
                return new ArgumentOutOfRangeException(nameof(count), "Non-negative number required.");

            Debug.Assert(array.Length - offset < count);
            return new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
        }

        private static void ThrowArgumentOutOfRange_IndexException()
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentOutOfRangeException("index",
                "Index was out of range. Must be non-negative and less than the size of the collection.");
        }

        private static void ThrowArgumentException_DestinationTooShort()
        {
            throw new ArgumentException("Destination is too short.");
        }

        private void ThrowInvalidOperationIfDefault()
        {
            if (_array == null)
            {
                throw new InvalidOperationException("The underlying array is null.");
            }
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] _array;
            private readonly int _start;
            private readonly int _end;
            private int _current;

            internal Enumerator(ReadOnlyArraySegment<T> arraySegment)
            {
                Debug.Assert(arraySegment.Array != null);
                Debug.Assert(arraySegment.Offset >= 0);
                Debug.Assert(arraySegment.Count >= 0);
                Debug.Assert(arraySegment.Offset + arraySegment.Count <= arraySegment.Array.Length);

                _array = arraySegment.Array;
                _start = arraySegment.Offset;
                _end = arraySegment.Offset + arraySegment.Count;
                _current = arraySegment.Offset - 1;
            }

            public bool MoveNext()
            {
                if (_current < _end)
                {
                    _current++;
                    return (_current < _end);
                }
                return false;
            }

            public T Current
            {
                get
                {
                    if (_current < _start)
                        throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");

                    if (_current >= _end)
                        throw new InvalidOperationException("Enumeration already finished.");

                    return _array[_current];
                }
            }

            object IEnumerator.Current => Current;

            void IEnumerator.Reset()
            {
                _current = _start - 1;
            }

            public void Dispose()
            {
            }
        }
    }
}
