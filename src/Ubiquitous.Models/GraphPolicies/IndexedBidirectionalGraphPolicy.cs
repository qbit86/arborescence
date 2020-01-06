namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedBidirectionalGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, int>,
        IGetTargetPolicy<TGraph, int, int>,
        IGetOutEdgesPolicy<TGraph, int, TEdges>,
        IGetInEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IBidirectionalGraph<int, int, TEdges>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public bool TryGetSource(TGraph graph, int edge, out int source)
        {
            return graph.TryGetSource(edge, out source);
        }

        public bool TryGetTarget(TGraph graph, int edge, out int target)
        {
            return graph.TryGetTarget(edge, out target);
        }

        public void TryGetOutEdges(TGraph graph, int vertex, out TEdges edges)
        {
            graph.TryGetOutEdges(vertex, out edges);
        }

        public bool TryGetInEdges(TGraph graph, int vertex, out TEdges edges)
        {
            return graph.TryGetInEdges(vertex, out edges);
        }
    }
}
