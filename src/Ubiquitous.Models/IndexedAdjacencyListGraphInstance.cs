namespace Ubiquitous
{
    public struct IndexedAdjacencyListGraphInstance :
        IGetTargetConcept<IndexedAdjacencyListGraph, int, int>,
        IIncidenceVertexConcept<IndexedAdjacencyListGraph, int, ImmutableArrayEnumeratorAdapter<int>>
    {
        public bool TryGetSource(IndexedAdjacencyListGraph graph, int edge, out int source)
        {
            SourceTargetPair<int> endpoints;
            bool result = graph.TryGetEndpoints(edge, out endpoints);
            source = result ? endpoints.Source : default(int);
            return result;
        }

        public bool TryGetTarget(IndexedAdjacencyListGraph graph, int edge, out int target)
        {
            SourceTargetPair<int> endpoints;
            bool result = graph.TryGetEndpoints(edge, out endpoints);
            target = result ? endpoints.Target : default(int);
            return result;
        }

        public bool TryGetOutEdges(IndexedAdjacencyListGraph graph, int vertex, out ImmutableArrayEnumeratorAdapter<int> edges)
        {
            return graph.TryGetOutEdges(vertex, out edges);
        }
    }
}
