namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs
    public struct ArraySegmentEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _array;
        private readonly int _start;
        private readonly int _end; // cache Offset + Count, since it's a little slow
        private int _current;

        internal ArraySegmentEnumerator(T[] array, int offset, int count)
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

        public ArraySegmentEnumerator<T> GetEnumerator()
        {
            return this;
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
