namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs
    public struct ArrayPrefixEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly T[] _array;
        private readonly int _end;
        private int _current;

        public ArrayPrefixEnumerator(T[] array, int count)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            if (array == null || (uint)count > (uint)array.Length)
                ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, 0, count);

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

        public ArrayPrefixEnumerator<T> GetEnumerator()
        {
            return this;
        }

        object IEnumerator.Current => Current;

        void IDisposable.Dispose()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }

        void IEnumerator.Reset()
        {
            _current = -1;
        }
    }
}
