namespace Ubiquitous
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct SortedAdjacencyListIncidenceGraph : IEquatable<SortedAdjacencyListIncidenceGraph>,
        IIncidenceGraph<int, int, RangeEnumerator>
    {
        // Layout:
        // [0..1) — VertexUpperBound
        // [1..Offset₁ + VertexUpperBound) — EdgeBounds; EdgeCount := (Length - 1 - VertexUpperBound) / 2
        // [1 + VertexUpperBound..Offset₂ + EdgeCount) — Targets
        // [1 + VertexUpperBound + EdgeCount..Offset₃ + EdgeCount) — Sources
        private readonly int[] _storage;

        internal SortedAdjacencyListIncidenceGraph(int[] storage)
        {
            Assert(storage != null, "storage != null");
            Assert(storage.Length > 0, "storage.Length > 0");

            Assert(storage[0] >= 0, "storage[0] >= 0");
            Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `endpoints` are sorted by source(edge).
            // Assert: `edgeBounds` are vertexUpperBound in length.
            // Assert: `edgeBounds` contain increasing indices pointing to Endpoints.

            _storage = storage;
        }

        public int VertexUpperBound => _storage == null ? 0 : GetVertexUpperBound();

        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - GetVertexUpperBound()) / 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgeBounds()
        {
            return _storage.AsSpan().Slice(1, VertexUpperBound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetSources()
        {
            return _storage.AsSpan().Slice(1 + VertexUpperBound + EdgeCount, EdgeCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTargets()
        {
            return _storage.AsSpan().Slice(1 + VertexUpperBound, EdgeCount);
        }

        public bool TryGetSource(int edge, out int source)
        {
            ReadOnlySpan<int> sources = GetSources();
            if ((uint)edge >= (uint)sources.Length)
            {
                source = -1;
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
                target = -1;
                return false;
            }

            target = targets[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out RangeEnumerator outEdges)
        {
            ReadOnlySpan<int> edgeBounds = GetEdgeBounds();
            if ((uint)vertex >= (uint)edgeBounds.Length)
            {
                outEdges = new RangeEnumerator(0, 0);
                return false;
            }

            int start = vertex > 0 ? edgeBounds[vertex - 1] : 0;
            int endExclusive = edgeBounds[vertex];
            if (endExclusive < start)
            {
                outEdges = new RangeEnumerator(0, 0);
                return false;
            }

            outEdges = new RangeEnumerator(start, endExclusive);
            return true;
        }

        public bool Equals(SortedAdjacencyListIncidenceGraph other)
        {
            return Equals(_storage, other._storage);
        }

        public override bool Equals(object obj)
        {
            if (obj is SortedAdjacencyListIncidenceGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return _storage?.GetHashCode() ?? 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetVertexUpperBound()
        {
            Assert(_storage != null, "_storage != null");
            Assert(_storage.Length > 0, "_storage.Length > 0");

            int result = _storage[0];
            Assert(result >= 0, "result >= 0");
            Assert(result <= _storage.Length - 1, "result <= _storage.Length - 1");

            return result;
        }
    }
}
