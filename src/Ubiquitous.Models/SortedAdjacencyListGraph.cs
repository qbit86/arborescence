namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public readonly struct SortedAdjacencyListGraph : IEquatable<SortedAdjacencyListGraph>,
        IIncidenceGraph<int, int, RangeEnumerator>
    {
        // Layout:
        // [0..1) — VertexCount
        // [1..Offset₁ + VertexCount) — EdgeBounds; EdgeCount := (Length - (1 + VertexCount)) / 2
        // [1 + VertexCount..Offset₂ + EdgeCount) — Sources
        // [1 + VertexCount + EdgeCount..Offset₃ + EdgeCount) — Targets

        internal SortedAdjacencyListGraph(int[] endpoints, int[] edgeBounds)
        {
            Assert(endpoints != null);
            Assert(edgeBounds != null);

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `endpoints` are sorted by source(edge).
            // Assert: `edgeBounds` are vertexCount in length.
            // Assert: `edgeBounds` contain increasing indices pointing to Endpoints.

            Endpoints = endpoints;
            EdgeBounds = edgeBounds;
        }

        public int VertexCount => EdgeBounds?.Length ?? 0;

        public int EdgeCount => Endpoints?.Length / 2 ?? 0;

        private int[] Endpoints { get; }

        private int[] EdgeBounds { get; }

        public bool TryGetSource(int edge, out int source)
        {
            int edgeCount = EdgeCount;

            if ((uint)edge >= (uint)edgeCount)
            {
                source = default;
                return false;
            }

            source = Endpoints[edgeCount + edge];
            return true;
        }

        public bool TryGetTarget(int edge, out int target)
        {
            if ((uint)edge >= (uint)EdgeCount)
            {
                target = default;
                return false;
            }

            target = Endpoints[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out RangeEnumerator outEdges)
        {
            if ((uint)vertex >= (uint)VertexCount)
            {
                outEdges = default;
                return false;
            }

            int start = vertex > 0 ? EdgeBounds[vertex - 1] : 0;
            int endExclusive = EdgeBounds[vertex];
            if (endExclusive < start)
            {
                outEdges = default;
                return false;
            }

            outEdges = new RangeEnumerator(start, endExclusive);
            return true;
        }

        public bool Equals(SortedAdjacencyListGraph other)
        {
            return Equals(Endpoints, other.Endpoints) && Equals(EdgeBounds, other.EdgeBounds);
        }

        public override bool Equals(object obj)
        {
            if (obj is SortedAdjacencyListGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return unchecked(((Endpoints?.GetHashCode() ?? 0) * 397) ^ (EdgeBounds?.GetHashCode() ?? 0));
        }
    }
}
