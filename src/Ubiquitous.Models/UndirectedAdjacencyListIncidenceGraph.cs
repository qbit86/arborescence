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

        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - 2 * GetVertexCount()) / 3;

        public bool TryGetSource(int edge, out int source)
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

        public bool TryGetTarget(int edge, out int target)
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

        public ArraySegmentEnumerator<int> EnumerateOutEdges(int vertex) => throw new NotImplementedException();

        public bool Equals(UndirectedAdjacencyListIncidenceGraph other) => _storage == other._storage;

        public override bool Equals(object obj) => obj is UndirectedAdjacencyListIncidenceGraph other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetSources() => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTargets() => throw new NotImplementedException();

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
    }
}
