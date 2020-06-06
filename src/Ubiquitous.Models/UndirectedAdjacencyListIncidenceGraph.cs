namespace Ubiquitous.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct UndirectedAdjacencyListIncidenceGraph :
        IIncidenceGraph<int, int, ArraySegmentEnumerator<int>>, IEquatable<UndirectedAdjacencyListIncidenceGraph>
    {
        private readonly int[] _storage;

        internal UndirectedAdjacencyListIncidenceGraph(int[] storage)
        {
            Assert(storage != null, "storage != null");
            Assert(storage.Length > 0, "storage.Length > 0");

            Assert(storage[0] >= 0, "storage[0] >= 0");
            Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            _storage = storage;
        }

        public int VertexCount => _storage == null ? 0 : GetVertexCount();

        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - 2 * GetVertexCount()) / 4;

        public bool TryGetTail(int edge, out int source)
        {
            int actualEdge = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> actualSources = edge < 0 ? GetTargets() : GetSources();
            if (actualEdge >= actualSources.Length)
            {
                source = default;
                return false;
            }

            source = actualSources[actualEdge];
            return true;
        }

        public bool TryGetHead(int edge, out int target)
        {
            int actualEdge = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> actualTargets = edge < 0 ? GetSources() : GetTargets();
            if (actualEdge >= actualTargets.Length)
            {
                target = default;
                return false;
            }

            target = actualTargets[actualEdge];
            return true;
        }

        public ArraySegmentEnumerator<int> EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<int> edgeBounds = GetEdgeBounds();

            if ((uint)(2 * vertex) >= (uint)edgeBounds.Length)
                return new ArraySegmentEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0, 0);

            int start = edgeBounds[2 * vertex];
            int length = edgeBounds[2 * vertex + 1];
            Assert(length >= 0, "length >= 0");

            return new ArraySegmentEnumerator<int>(_storage, start, length);
        }

        public bool Equals(UndirectedAdjacencyListIncidenceGraph other) => _storage == other._storage;

        public override bool Equals(object obj) => obj is UndirectedAdjacencyListIncidenceGraph other && Equals(other);

        public override int GetHashCode() => _storage?.GetHashCode() ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgeBounds() => _storage.AsSpan(1, 2 * VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetSources() => _storage.AsSpan(1 + 2 * VertexCount + 3 * EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTargets() => _storage.AsSpan(1 + 2 * VertexCount + 2 * EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetVertexCount()
        {
            Assert(_storage != null, "_storage != null");
            Assert(_storage.Length > 0, "_storage.Length > 0");

            int result = _storage[0];
            Assert(result >= 0, "result >= 0");
            Assert(2 * result <= _storage.Length - 1, "2 * result <= _storage.Length - 1");

            return result;
        }

        public static bool operator ==(UndirectedAdjacencyListIncidenceGraph left,
            UndirectedAdjacencyListIncidenceGraph right) => left.Equals(right);

        public static bool operator !=(UndirectedAdjacencyListIncidenceGraph left,
            UndirectedAdjacencyListIncidenceGraph right) => !left.Equals(right);
    }
}
