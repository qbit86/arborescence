namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using static System.Diagnostics.Debug;

    public struct IndexedAdjacencyListGraph : IEquatable<IndexedAdjacencyListGraph>
    {
        private List<SourceTargetPair<int>> Endpoints { get; }
        private ImmutableArray<int>[] OutEdges { get; }

        internal IndexedAdjacencyListGraph(
            List<SourceTargetPair<int>> endpoints,
            ImmutableArray<int>[] outEdges)
        {
            Assert(endpoints != null);
            Assert(outEdges != null);

            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `outEdges` are consistent. For each vertex and for each edge in outEdges(vertex): source(edge) = vertex.

            Endpoints = endpoints;
            OutEdges = outEdges;
        }

        public int VertexCount => OutEdges.Length;

        public int EdgeCount => Endpoints.Count;

        public bool TryGetEndpoints(int edge, out SourceTargetPair<int> endpoints)
        {
            if (edge < 0 || edge >= Endpoints.Count)
            {
                endpoints = default;
                return false;
            }

            endpoints = Endpoints[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out ImmutableArrayEnumeratorAdapter<int> outEdges)
        {
            if (vertex < 0 || vertex >= VertexCount)
            {
                outEdges = default;
                return false;
            }

            outEdges = new ImmutableArrayEnumeratorAdapter<int>(OutEdges[vertex].GetEnumerator());
            return true;
        }

        public bool Equals(IndexedAdjacencyListGraph other)
        {
            if (!Endpoints.Equals(other.Endpoints))
                return false;

            if (!OutEdges.Equals(other.OutEdges))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is IndexedAdjacencyListGraph other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return Endpoints.GetHashCode() ^ OutEdges.GetHashCode();
        }
    }
}
