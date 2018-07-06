namespace Ubiquitous
{
    public struct IndexedAdjacencyListGraphInstance :
        IGetTargetConcept<IndexedAdjacencyListGraph, int, int>,
        IGetOutEdgesConcept<IndexedAdjacencyListGraph, int, ImmutableArrayEnumeratorAdapter<int>>
    {
        public bool TryGetSource(IndexedAdjacencyListGraph graph, int edge, out int source)
        {
            bool result = graph.TryGetEndpoints(edge, out SourceTargetPair<int> endpoints);
            source = result ? endpoints.Source : default;
            return result;
        }

        public bool TryGetTarget(IndexedAdjacencyListGraph graph, int edge, out int target)
        {
            bool result = graph.TryGetEndpoints(edge, out SourceTargetPair<int> endpoints);
            target = result ? endpoints.Target : default;
            return result;
        }

        public bool TryGetOutEdges(IndexedAdjacencyListGraph graph, int vertex,
            out ImmutableArrayEnumeratorAdapter<int> edges)
        {
            return graph.TryGetOutEdges(vertex, out edges);
        }
    }
}
