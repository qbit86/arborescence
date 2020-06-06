namespace Ubiquitous.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct EdgeListIncidenceGraph :
        IIncidenceGraph<int, Endpoints<int>, ArraySegmentEnumerator<Endpoints<int>>>,
        IEquatable<EdgeListIncidenceGraph>
    {
        private readonly Endpoints<int>[] _storage;

        internal EdgeListIncidenceGraph(int vertexCount, Endpoints<int>[] storage)
        {
            Assert(vertexCount >= 0, "vertexCount >= 0");
            Assert(storage != null, "storage != null");

            _storage = storage;
            VertexCount = vertexCount;
        }

        public int VertexCount { get; }

        public int EdgeCount => _storage?.Length - VertexCount ?? 0;

        public bool TryGetTail(Endpoints<int> edge, out int source)
        {
            if ((uint)edge.Tail >= (uint)VertexCount)
            {
                source = default;
                return false;
            }

            if ((uint)edge.Head >= (uint)VertexCount)
            {
                source = default;
                return false;
            }

            source = edge.Tail;
            return true;
        }

        public bool TryGetHead(Endpoints<int> edge, out int target)
        {
            if ((uint)edge.Tail >= (uint)VertexCount)
            {
                target = default;
                return false;
            }

            if ((uint)edge.Head >= (uint)VertexCount)
            {
                target = default;
                return false;
            }

            target = edge.Head;
            return true;
        }

        public ArraySegmentEnumerator<Endpoints<int>> EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<Endpoints<int>> edgeBounds = GetEdgeBounds();
            if ((uint)vertex >= (uint)edgeBounds.Length)
            {
                return new ArraySegmentEnumerator<Endpoints<int>>(
                    ArrayBuilder<Endpoints<int>>.EmptyArray, 0, 0);
            }

            int start = edgeBounds[vertex].Tail;
            int length = edgeBounds[vertex].Head;
            return new ArraySegmentEnumerator<Endpoints<int>>(_storage, start, length);
        }

        public bool Equals(EdgeListIncidenceGraph other) =>
            VertexCount == other.VertexCount && _storage == other._storage;

        public override bool Equals(object obj) => obj is EdgeListIncidenceGraph other && Equals(other);

        public override int GetHashCode() => unchecked(VertexCount * 397) ^ (_storage?.GetHashCode() ?? 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<Endpoints<int>> GetEdgeBounds() => _storage.AsSpan(EdgeCount, VertexCount);

        public static bool operator ==(EdgeListIncidenceGraph left, EdgeListIncidenceGraph right) =>
            left.Equals(right);

        public static bool operator !=(EdgeListIncidenceGraph left, EdgeListIncidenceGraph right) =>
            !left.Equals(right);
    }
}
