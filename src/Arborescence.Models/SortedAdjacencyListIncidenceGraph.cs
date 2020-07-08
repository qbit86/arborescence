namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public readonly struct SortedAdjacencyListIncidenceGraph : IIncidenceGraph<int, int, RangeEnumerator>,
        IEquatable<SortedAdjacencyListIncidenceGraph>
    {
        // Layout:
        // [0..1) — VertexCount
        // [1..Offset₁ + VertexCount) — EdgeBounds; EdgeCount := (Length - 1 - VertexCount) / 2
        // [1 + VertexCount..Offset₂ + EdgeCount) — Heads
        // [1 + VertexCount + EdgeCount..Offset₃ + EdgeCount) — Tails
        private readonly int[] _storage;

        internal SortedAdjacencyListIncidenceGraph(int[] storage)
        {
            Assert(storage != null, "storage != null");
            Assert(storage.Length > 0, "storage.Length > 0");

            Assert(storage[0] >= 0, "storage[0] >= 0");
            Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            // Assert: `endpoints` are consistent. For each edge: tail(edge) and head(edge) belong to vertices.
            // Assert: `endpoints` are sorted by tail(edge).
            // Assert: `edgeBounds` are vertexCount in length.
            // Assert: `edgeBounds` contain increasing indices pointing to Endpoints.

            _storage = storage;
        }

        public int VertexCount => _storage == null ? 0 : GetVertexCount();

        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - GetVertexCount()) / 2;

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail)
        {
            ReadOnlySpan<int> tails = GetTails();
            if ((uint)edge >= (uint)tails.Length)
            {
                tail = default;
                return false;
            }

            tail = tails[edge];
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            ReadOnlySpan<int> heads = GetHeads();
            if ((uint)edge >= (uint)heads.Length)
            {
                head = default;
                return false;
            }

            head = heads[edge];
            return true;
        }

        /// <inheritdoc/>
        public RangeEnumerator EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<int> edgeBounds = GetEdgeBounds();
            if ((uint)vertex >= (uint)edgeBounds.Length)
                return new RangeEnumerator(0, 0);

            int start = vertex > 0 ? edgeBounds[vertex - 1] : 0;
            int endExclusive = edgeBounds[vertex];
            Assert(start <= endExclusive, "start <= endExclusive");

            return new RangeEnumerator(start, endExclusive);
        }

        /// <inheritdoc/>
        public bool Equals(SortedAdjacencyListIncidenceGraph other) => Equals(_storage, other._storage);

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            obj is SortedAdjacencyListIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => _storage?.GetHashCode() ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgeBounds() => _storage.AsSpan(1, VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTails() => _storage.AsSpan(1 + VertexCount + EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetHeads() => _storage.AsSpan(1 + VertexCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetVertexCount()
        {
            Assert(_storage != null, "_storage != null");
            Assert(_storage.Length > 0, "_storage.Length > 0");

            int result = _storage[0];
            Assert(result >= 0, "result >= 0");
            Assert(result <= _storage.Length - 1, "result <= _storage.Length - 1");

            return result;
        }

        public static bool operator ==(SortedAdjacencyListIncidenceGraph left,
            SortedAdjacencyListIncidenceGraph right) => left.Equals(right);

        public static bool operator !=(SortedAdjacencyListIncidenceGraph left,
            SortedAdjacencyListIncidenceGraph right) => !left.Equals(right);
    }
}
