namespace Ubiquitous
{
    using System.Collections.Generic;

    public readonly struct IndexedAdjacencyListGraphPolicy :
        IGetTargetPolicy<IndexedAdjacencyListGraph, int, int>,
        IGetOutEdgesPolicy<IndexedAdjacencyListGraph, int, List<int>.Enumerator>
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
            out List<int>.Enumerator edges)
        {
            return graph.TryGetOutEdges(vertex, out edges);
        }
    }
}
