namespace Arborescence
{
    using System.Collections;
    using System.Collections.Generic;

    public struct IndexEnumerator : IEnumerator<int>
    {
        private readonly int _count;
        private int _current;

        public IndexEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        public IndexEnumerator GetEnumerator()
        {
            return this;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _current = -1;
        }

        object IEnumerator.Current => _current;

        /// <inheritdoc />
        public int Current => _current;

        /// <inheritdoc />
        public bool MoveNext()
        {
            ++_current;
            return _current < _count;
        }

        /// <inheritdoc />
        public void Dispose() { }
    }
}
