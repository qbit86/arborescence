namespace Ubiquitous.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct EdgeListIncidenceGraph :
        IIncidenceGraph<int, SourceTargetPair<int>, ArraySegmentEnumerator<SourceTargetPair<int>>>,
        IEquatable<EdgeListIncidenceGraph>
    {
        private readonly SourceTargetPair<int>[] _storage;

        internal EdgeListIncidenceGraph(int vertexCount, SourceTargetPair<int>[] storage)
        {
            Assert(vertexCount >= 0, "vertexCount >= 0");
            Assert(storage != null, "storage != null");

            _storage = storage;
            VertexCount = vertexCount;
        }

        public int VertexCount { get; }

        public int EdgeCount => _storage?.Length - VertexCount ?? 0;

        public bool TryGetSource(SourceTargetPair<int> edge, out int source)
        {
            if ((uint)edge.Source >= (uint)VertexCount)
            {
                source = -1;
                return false;
            }

            if ((uint)edge.Target >= (uint)VertexCount)
            {
                source = -2;
                return false;
            }

            source = edge.Source;
            return true;
        }

        public bool TryGetTarget(SourceTargetPair<int> edge, out int target)
        {
            if ((uint)edge.Source >= (uint)VertexCount)
            {
                target = -1;
                return false;
            }

            if ((uint)edge.Target >= (uint)VertexCount)
            {
                target = -2;
                return false;
            }

            target = edge.Target;
            return true;
        }

        public ArraySegmentEnumerator<SourceTargetPair<int>> EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<SourceTargetPair<int>> edgeBounds = GetEdgeBounds();
            if ((uint)vertex >= (uint)edgeBounds.Length)
            {
                return new ArraySegmentEnumerator<SourceTargetPair<int>>(
                    ArrayBuilder<SourceTargetPair<int>>.EmptyArray, 0, 0);
            }

            int start = edgeBounds[vertex].Source;
            int length = edgeBounds[vertex].Target;
            return new ArraySegmentEnumerator<SourceTargetPair<int>>(_storage, start, length);
        }

        public bool Equals(EdgeListIncidenceGraph other)
        {
            return VertexCount == other.VertexCount && _storage == other._storage;
        }

        public override bool Equals(object obj)
        {
            return obj is EdgeListIncidenceGraph other && Equals(other);
        }

        public override int GetHashCode()
        {
            return unchecked(VertexCount * 397) ^ (_storage?.GetHashCode() ?? 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<SourceTargetPair<int>> GetEdgeBounds()
        {
            return _storage.AsSpan(EdgeCount, VertexCount);
        }

        public static bool operator ==(EdgeListIncidenceGraph left, EdgeListIncidenceGraph right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EdgeListIncidenceGraph left, EdgeListIncidenceGraph right)
        {
            return !left.Equals(right);
        }
    }
}
