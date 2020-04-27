namespace Ubiquitous.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct SortedAdjacencyListIncidenceGraph : IIncidenceGraph<int, int, RangeEnumerator>,
        IEquatable<SortedAdjacencyListIncidenceGraph>
    {
        // Layout:
        // [0..1) — VertexCount
        // [1..Offset₁ + VertexCount) — EdgeBounds; EdgeCount := (Length - 1 - VertexCount) / 2
        // [1 + VertexCount..Offset₂ + EdgeCount) — Targets
        // [1 + VertexCount + EdgeCount..Offset₃ + EdgeCount) — Sources
        private readonly int[] _storage;

        internal SortedAdjacencyListIncidenceGraph(int[] storage)
        {
            Assert(storage != null, "storage != null");
            Assert(storage.Length > 0, "storage.Length > 0");

            Assert(storage[0] >= 0, "storage[0] >= 0");
            Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `endpoints` are sorted by source(edge).
            // Assert: `edgeBounds` are vertexCount in length.
            // Assert: `edgeBounds` contain increasing indices pointing to Endpoints.

            _storage = storage;
        }

        public int VertexCount => _storage == null ? 0 : GetVertexCount();

        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - GetVertexCount()) / 2;

        public bool TryGetSource(int edge, out int source)
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

        public bool TryGetTarget(int edge, out int target)
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

        public bool Equals(SortedAdjacencyListIncidenceGraph other) => Equals(_storage, other._storage);

        public override bool Equals(object obj) =>
            obj is SortedAdjacencyListIncidenceGraph other && Equals(other);

        public override int GetHashCode() => _storage?.GetHashCode() ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgeBounds() => _storage.AsSpan(1, VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetSources() => _storage.AsSpan(1 + VertexCount + EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTargets() => _storage.AsSpan(1 + VertexCount, EdgeCount);

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
