namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct GenericIncidenceGraphPolicy<TGraph, TVertex, TEdge, TEdges> :
        IGetSourcePolicy<TGraph, TVertex, TEdge>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IOutEdgesPolicy<TGraph, TVertex, TEdges>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdges>
    {
        public bool TryGetTail(TGraph graph, TEdge edge, out TVertex source) => graph.TryGetTail(edge, out source);

        public bool TryGetHead(TGraph graph, TEdge edge, out TVertex target) => graph.TryGetHead(edge, out target);

        public TEdges EnumerateOutEdges(TGraph graph, TVertex vertex) => graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
