// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter

namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public readonly struct IndexCollection : IReadOnlyCollection<int>, IEquatable<IndexCollection>
    {
        public int Count { get; }

        public IndexCollection(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        public bool Equals(IndexCollection other)
        {
            if (Count != other.Count)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is IndexCollection other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Count.GetHashCode();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(Count);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return new Enumerator(Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(Count);
        }

        public struct Enumerator : IEnumerator<int>
        {
            private readonly int _count;
            private int _current;

            public Enumerator(int count)
            {
                _count = count;
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
                return _current < _count;
            }

            public void Dispose()
            {
                _current = _count;
            }
        }
    }
}
