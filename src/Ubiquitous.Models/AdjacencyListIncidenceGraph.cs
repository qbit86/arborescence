namespace Ubiquitous.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct AdjacencyListIncidenceGraph : IIncidenceGraph<int, int, ArraySegmentEnumerator<int>>,
        IEquatable<AdjacencyListIncidenceGraph>
    {
        private readonly int[] _storage;

        internal AdjacencyListIncidenceGraph(int[] storage)
        {
            Assert(storage != null, "storage != null");
            Assert(storage.Length > 0, "storage.Length > 0");

            Assert(storage[0] >= 0, "storage[0] >= 0");
            Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            _storage = storage;
        }

        public int VertexCount => _storage == null ? 0 : GetVertexCount();

        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - 2 * GetVertexCount()) / 3;

        public bool TryGetTail(int edge, out int source)
        {
            ReadOnlySpan<int> sources = GetSources();
            if ((uint)edge >= (uint)sources.Length)
            {
                source = default;
                return false;
            }

            source = sources[edge];
            return true;
        }

        public bool TryGetHead(int edge, out int target)
        {
            ReadOnlySpan<int> targets = GetTargets();
            if ((uint)edge >= (uint)targets.Length)
            {
                target = default;
                return false;
            }

            target = targets[edge];
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

        public bool Equals(AdjacencyListIncidenceGraph other) => _storage == other._storage;

        public override bool Equals(object obj) => obj is AdjacencyListIncidenceGraph other && Equals(other);

        public override int GetHashCode() => _storage?.GetHashCode() ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgeBounds() => _storage.AsSpan(1, 2 * VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetSources() => _storage.AsSpan(1 + 2 * VertexCount + 2 * EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTargets() => _storage.AsSpan(1 + 2 * VertexCount + EdgeCount, EdgeCount);

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

        public static bool operator ==(AdjacencyListIncidenceGraph left, AdjacencyListIncidenceGraph right) =>
            left.Equals(right);

        public static bool operator !=(AdjacencyListIncidenceGraph left, AdjacencyListIncidenceGraph right) =>
            !left.Equals(right);
    }
}
