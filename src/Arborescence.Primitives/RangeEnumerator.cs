namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;

    public struct RangeEnumerator : IEnumerator<int>
    {
        private readonly int _start;
        private readonly int _end;
        private int _current;

        public RangeEnumerator(int start, int endExclusive)
        {
            if (endExclusive < start)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);

            _start = start;
            _end = endExclusive;
            _current = start - 1;
        }

        public RangeEnumerator GetEnumerator()
        {
            return this;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _current = _start - 1;
        }

        object IEnumerator.Current => _current;

        /// <inheritdoc />
        public int Current => _current;

        /// <inheritdoc />
        public bool MoveNext()
        {
            ++_current;
            return _current < _end;
        }

        /// <inheritdoc />
        public void Dispose() { }
    }
}
