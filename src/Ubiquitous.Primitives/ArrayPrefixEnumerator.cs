namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/ArraySegment.cs
    public struct ArrayPrefixEnumerator<T> : IEnumerator<T>
    {
        private const int Start = 0;
        private readonly T[] _array;
        private readonly int _end;
        private int _current;

        public ArrayPrefixEnumerator(T[] array, int count)
        {
            if (array == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);

            const int offset = 0;

            if (array == null || (uint)count > (uint)(array.Length - offset))
                ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, offset, count);

            _array = array;
            _end = offset + count;
            _current = offset - 1;
        }

        public T Current
        {
            get
            {
                if (_current < Start)
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

        void IEnumerator.Reset()
        {
            _current = Start - 1;
        }

        public void Dispose()
        {
        }
    }
}
