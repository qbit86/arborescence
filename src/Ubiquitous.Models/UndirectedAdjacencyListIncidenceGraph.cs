namespace Ubiquitous.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct UndirectedAdjacencyListIncidenceGraph :
        IIncidenceGraph<int, int, ArraySegmentEnumerator<int>>, IEquatable<AdjacencyListIncidenceGraph>
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

        public int EdgeCount => _storage == null ? 0 : 2 * (_storage.Length - 1 - 2 * GetVertexCount()) / 3;

        public bool TryGetSource(int edge, out int source) => throw new NotImplementedException();

        public bool TryGetTarget(int edge, out int target) => throw new NotImplementedException();

        public ArraySegmentEnumerator<int> EnumerateOutEdges(int vertex) => throw new NotImplementedException();

        public bool Equals(AdjacencyListIncidenceGraph other) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetSources()
        {
            return _storage.AsSpan(1 + 2 * VertexCount + 2 * EdgeCount, EdgeCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTargets()
        {
            return _storage.AsSpan(1 + 2 * VertexCount + EdgeCount, EdgeCount);
        }

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
