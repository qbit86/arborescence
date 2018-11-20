namespace Ubiquitous
{
    public readonly struct GenericTargetOnlyIncidenceGraphPolicy<TGraph, TVertex, TEdge, TEdges> :
        IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IGetOutEdgesPolicy<TGraph, TVertex, TEdges>
        where TGraph : IGetTarget<TVertex, TEdge>, IGetOutEdges<TVertex, TEdges>
    {
        public bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target)
        {
            return graph.TryGetTarget(edge, out target);
        }

        public bool TryGetOutEdges(TGraph graph, TVertex vertex, out TEdges edges)
        {
            return graph.TryGetOutEdges(vertex, out edges);
        }
    }
}
