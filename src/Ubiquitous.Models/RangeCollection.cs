// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter

namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public readonly struct RangeCollection : IReadOnlyCollection<int>, IEquatable<RangeCollection>
    {
        public int Count { get; }

        public RangeCollection(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        public bool Equals(RangeCollection other)
        {
            if (Count != other.Count)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is RangeCollection other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Count.GetHashCode();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<int>
        {
            private readonly RangeCollection _range;
            private int _current;

            public Enumerator(RangeCollection range)
            {
                _range = range;
                _current = -1;
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
                return _current < _range.Count;
            }

            public void Dispose()
            {
                _current = _range.Count;
            }
        }
    }
}
