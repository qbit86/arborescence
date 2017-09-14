namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct RangeCollection : IReadOnlyCollection<int>, IEquatable<RangeCollection>
    {
        public int Start { get; }

        public int Count { get; }

        public RangeCollection(int start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Start = start;
            Count = count;
        }

        public bool Equals(RangeCollection other)
        {
            if (Start != other.Start || Count != other.Count)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RangeCollection))
                return false;

            var other = (RangeCollection)obj;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ Count.GetHashCode();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<int>
        {
            RangeCollection _range;
            private int _current;

            public Enumerator(RangeCollection range)
            {
                _range = range;
                _current = range.Start - 1;
            }

            public int Current => _current;

            object System.Collections.IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                ++_current;
                return _current < _range.Start + _range.Count;
            }

            public void Reset()
            {
                _current = _range.Start - 1;
            }
        }
    }
}
