namespace Arborescence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <inheritdoc/>
    public struct RangeEnumerator : IEnumerator<int>
    {
        private readonly int _start;
        private readonly int _end;
        private int _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeEnumerator"/> structure.
        /// </summary>
        /// <param name="start">The zero-based index of the first element in the range.</param>
        /// <param name="endExclusive">The zero-based index of the element after the last.</param>
        public RangeEnumerator(int start, int endExclusive)
        {
            if (endExclusive < start)
                ThrowArgumentOutOfRangeException(nameof(start));

            _start = start;
            _end = endExclusive;
            _current = start - 1;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator for the range of elements.</returns>
        public readonly RangeEnumerator GetEnumerator()
        {
            RangeEnumerator ator = this;
            ator._current = _start - 1;
            return ator;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            _current = _start - 1;
        }

        object IEnumerator.Current => _current;

        /// <inheritdoc/>
        public readonly int Current => _current;

        /// <inheritdoc/>
        public bool MoveNext()
        {
            ++_current;
            return _current < _end;
        }

        /// <inheritdoc/>
        public void Dispose() { }

        private static void ThrowArgumentOutOfRangeException(string argument)
        {
            throw new ArgumentOutOfRangeException(argument);
        }
    }
}
