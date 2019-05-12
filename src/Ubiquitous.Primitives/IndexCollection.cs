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
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count);

            Count = count;
        }

#pragma warning disable CA1024 // Use properties where appropriate
        public IndexCollectionEnumerator GetValueEnumerator()
        {
            return new IndexCollectionEnumerator(Count);
        }
#pragma warning restore CA1024 // Use properties where appropriate

        public Enumerator GetEnumerator()
        {
            return new Enumerator(Count);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return new IndexCollectionEnumerator(Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new IndexCollectionEnumerator(Count);
        }

        public bool Equals(IndexCollection other)
        {
            if (Count != other.Count)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is IndexCollection other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Count.GetHashCode();
        }

        public static bool operator ==(IndexCollection left, IndexCollection right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IndexCollection left, IndexCollection right)
        {
            return !left.Equals(right);
        }


        // https://github.com/dotnet/corefx/blob/master/src/System.Collections.Immutable/src/System/Collections/Immutable/ImmutableArray_1.Enumerator.cs

#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable CA1034 // Nested types should not be visible
        /// <summary>
        /// An index enumerator.
        /// </summary>
        /// <remarks>
        /// It is important that this enumerator does NOT implement <see cref="IDisposable" />.
        /// We want the iterator to inline when we do foreach and to not result in
        /// a try/finally frame in the client.
        /// </remarks>
        public struct Enumerator
        {
            private readonly int _count;
            private int _current;

            internal Enumerator(int count)
            {
                _count = count;
                _current = -1;
            }

            public int Current => _current;

            public bool MoveNext()
            {
                ++_current;
                return _current < _count;
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
#pragma warning restore CA1815 // Override equals and operator equals on value types
    }
}
