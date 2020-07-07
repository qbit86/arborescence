namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs

#pragma warning disable CA1710 // Identifiers should have correct suffix
    /// <inheritdoc cref="System.Collections.Generic.IEnumerator{T}"/>
    public struct ArraySegmentEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly T[] _array;
        private readonly int _start;
        private readonly int _end; // cache Offset + Count, since it's a little slow
        private int _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArraySegmentEnumerator{T}"/> structure.
        /// </summary>
        /// <param name="array">The array containing the range of elements to enumerate.</param>
        /// <param name="offset">The zero-based index of the first element in the range.</param>
        /// <param name="count">The number of elements in the range.</param>
        public ArraySegmentEnumerator(T[] array, int offset, int count)
        {
            if (array == null || (uint)offset > (uint)array.Length || (uint)count > (uint)(array.Length - offset))
                ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, offset, count);

            _array = array;
            _start = offset;
            _end = offset + count;
            _current = offset - 1;
        }

        /// <inheritdoc/>
        public T Current
        {
            get
            {
                if (_current < _start)
                    ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
                if (_current >= _end)
                    ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
                return _array[_current];
            }
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (_current < _end)
            {
                _current++;
                return _current < _end;
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator for the range of elements.</returns>
        public ArraySegmentEnumerator<T> GetEnumerator()
        {
            ArraySegmentEnumerator<T> ator = this;
            ator._current = _start - 1;
            return ator;
        }

        object IEnumerator.Current => Current;

        void IEnumerator.Reset()
        {
            _current = _start - 1;
        }

        /// <inheritdoc/>
        public void Dispose() { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}
