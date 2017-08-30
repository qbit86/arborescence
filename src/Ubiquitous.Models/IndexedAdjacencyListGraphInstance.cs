namespace Ubiquitous
{
    using System.Collections.Generic;

    public struct IndexedAdjacencyListGraphInstance : IGraphConcept<
        IndexedAdjacencyListGraph, int, int, IEnumerable<int>, SourceTargetPair<int>>
    {
        public bool TryGetEdgeData(IndexedAdjacencyListGraph graph, int edge, out SourceTargetPair<int> edgeData)
            => graph.TryGetEndpoints(edge, out edgeData);

        public bool TryGetVertexData(IndexedAdjacencyListGraph graph, int vertex, out IEnumerable<int> vertexData)
            => graph.TryGetOutEdges(vertex, out vertexData);
    }
}
