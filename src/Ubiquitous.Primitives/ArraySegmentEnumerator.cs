namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct ArraySegmentEnumerator<T> : IEnumerator<T>, IEnumerable<T>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        private readonly T[] _array;
        private readonly int _start;
        private readonly int _end; // cache Offset + Count, since it's a little slow
        private int _current;

        public ArraySegmentEnumerator(T[] array, int offset, int count)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            if (array == null || (uint)offset > (uint)array.Length || (uint)count > (uint)(array.Length - offset))
                ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, offset, count);

            _array = array;
            _start = offset;
            _end = offset + count;
            _current = offset - 1;
        }

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

        public bool MoveNext()
        {
            if (_current < _end)
            {
                _current++;
                return _current < _end;
            }

            return false;
        }

        public ArraySegmentEnumerator<T> GetEnumerator()
        {
            return this;
        }

        object IEnumerator.Current => Current;

        void IEnumerator.Reset()
        {
            _current = _start - 1;
        }

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
}
