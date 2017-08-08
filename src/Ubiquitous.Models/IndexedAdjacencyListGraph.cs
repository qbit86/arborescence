namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct IndexedAdjacencyListGraph : IEquatable<IndexedAdjacencyListGraph>
    {
        private IReadOnlyList<SourceTargetPair<int>> Endpoints { get; }
        private IReadOnlyList<IEnumerable<int>> OutEdges { get; }

        public IndexedAdjacencyListGraph(
            IReadOnlyList<SourceTargetPair<int>> endpoints,
            IReadOnlyList<IEnumerable<int>> outEdges)
        {
            // Assert: `endpoints` are consistent. For each edge: source(edge) and target(edge) belong to vertices.
            // Assert: `outEdges` are consistent. For each vertex and for each edge in outEdges(vertex): source(edge) = vertex.

            Endpoints = endpoints;
            OutEdges = outEdges;
        }

        public IEnumerable<int> GetVertices() => Enumerable.Range(0, OutEdges.Count);

        public IEnumerable<int> GetEdges() => Enumerable.Range(0, Endpoints.Count);

        public bool TryGetEndpoints(int edge, out SourceTargetPair<int> endpoints)
        {
            if (edge < 0 || edge >= Endpoints.Count)
            {
                endpoints = default(SourceTargetPair<int>);
                return false;
            }

            endpoints = Endpoints[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out IEnumerable<int> outEdges)
        {
            if (vertex < 0 || vertex >= OutEdges.Count)
            {
                outEdges = null;
                return false;
            }

            outEdges = OutEdges[vertex];
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
            if (!(obj is IndexedAdjacencyListGraph))
                return false;

            var other = (IndexedAdjacencyListGraph)obj;
            return Equals(other);
        }

        public override int GetHashCode() => Endpoints.GetHashCode() ^ OutEdges.GetHashCode();
    }


    public struct IndexedAdjacencyListGraphInstance : IGraphConcept<
        IndexedAdjacencyListGraph, int, int, IEnumerable<int>, SourceTargetPair<int>>
    {
        public bool TryGetEdgeValue(IndexedAdjacencyListGraph graph, int edge, out SourceTargetPair<int> edgeValue)
            => graph.TryGetEndpoints(edge, out edgeValue);

        public bool TryGetVertexValue(IndexedAdjacencyListGraph graph, int vertex, out IEnumerable<int> vertexValue)
            => graph.TryGetOutEdges(vertex, out vertexValue);
    }
}
