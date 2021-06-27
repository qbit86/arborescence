namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;

    /// <inheritdoc/>
    public struct IndexEnumerator : IEnumerator<int>
    {
        private readonly int _count;
        private int _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexEnumerator"/> structure.
        /// </summary>
        /// <param name="count">The number of elements in the range.</param>
        public IndexEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator for the range of elements.</returns>
        public readonly IndexEnumerator GetEnumerator()
        {
            IndexEnumerator ator = this;
            ator._current = -1;
            return ator;
        }

        /// <inheritdoc/>
        public void Reset() => _current = -1;

        readonly object IEnumerator.Current => _current;

        /// <inheritdoc/>
        public readonly int Current => _current;

        /// <inheritdoc/>
        public bool MoveNext()
        {
            ++_current;
            return _current < _count;
        }

        /// <inheritdoc/>
        public readonly void Dispose() { }
    }
}
