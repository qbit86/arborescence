namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct GenericIncidenceGraphPolicy<TGraph, TVertex, TEdge, TEdges> :
        IGetSourcePolicy<TGraph, TVertex, TEdge>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IGetOutEdgesPolicy<TGraph, TVertex, TEdges>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdges>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public bool TryGetSource(TGraph graph, TEdge edge, out TVertex source)
        {
            return graph.TryGetSource(edge, out source);
        }

        public bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target)
        {
            return graph.TryGetTarget(edge, out target);
        }

        public void TryGetOutEdges(TGraph graph, TVertex vertex, out TEdges edges)
        {
            graph.TryGetOutEdges(vertex, out edges);
        }
    }
}
