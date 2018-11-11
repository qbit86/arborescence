namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;

    public struct RangeEnumerator : IEnumerator<int>
    {
        private readonly int _start;
        private readonly int _end;
        private int _current;

        public RangeEnumerator(int start, int count)
        {
            if (count < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count);

            _start = start;
            _end = start + count;
            _current = start - 1;
        }

        public void Reset()
        {
            _current = _start - 1;
        }

        object IEnumerator.Current => _current;

        public int Current => _current;

        public bool MoveNext()
        {
            ++_current;
            return _current < _end;
        }

        public void Dispose()
        {
        }
    }
}
