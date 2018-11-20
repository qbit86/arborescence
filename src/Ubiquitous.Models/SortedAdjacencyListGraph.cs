namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public readonly struct SortedAdjacencyListGraph : IEquatable<SortedAdjacencyListGraph>,
        IGetEndpoints<int, int>, IGetOutEdges<int, RangeEnumerator>
    {
        private ArrayPrefix<SourceTargetPair<int>> Endpoints { get; }
        private int[] EdgeBounds { get; }

        internal SortedAdjacencyListGraph(ArrayPrefix<SourceTargetPair<int>> endpoints, int[] edgeBounds)
        {
            Assert(endpoints.Array != null);
            Assert(edgeBounds != null);

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `endpoints` are sorted by source(edge).
            // Assert: `edgeBounds` are vertexCount in length.
            // Assert: `edgeBounds` contain increasing indices pointing to Endpoints.

            Endpoints = endpoints;
            EdgeBounds = edgeBounds;
        }

        public int VertexCount => EdgeBounds.Length;

        public int EdgeCount => Endpoints.Count;

        public bool TryGetEndpoints(int edge, out SourceTargetPair<int> endpoints)
        {
            if ((uint)edge >= (uint)EdgeCount)
            {
                endpoints = default;
                return false;
            }

            endpoints = Endpoints.Array[edge];
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
            if (!Endpoints.Equals(other.Endpoints))
                return false;

            if (!EdgeBounds.Equals(other.EdgeBounds))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is SortedAdjacencyListGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Endpoints.GetHashCode() ^ EdgeBounds.GetHashCode();
        }
    }
}
