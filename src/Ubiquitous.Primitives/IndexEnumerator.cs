namespace Ubiquitous
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

        public void Reset()
        {
            _current = -1;
        }

        object IEnumerator.Current => _current;

        public int Current => _current;

        public bool MoveNext()
        {
            ++_current;
            return _current < _count;
        }

        public void Dispose() { }
    }
}
