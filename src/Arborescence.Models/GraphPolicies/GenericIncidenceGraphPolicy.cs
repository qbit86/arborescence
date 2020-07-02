namespace Arborescence.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct GenericIncidenceGraphPolicy<TGraph, TVertex, TEdge, TEdges> :
        ITailPolicy<TGraph, TVertex, TEdge>,
        IHeadPolicy<TGraph, TVertex, TEdge>,
        IOutEdgesPolicy<TGraph, TVertex, TEdges>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdges>
    {
        public bool TryGetTail(TGraph graph, TEdge edge, out TVertex tail) => graph.TryGetTail(edge, out tail);

        public bool TryGetHead(TGraph graph, TEdge edge, out TVertex head) => graph.TryGetHead(edge, out head);

        public TEdges EnumerateOutEdges(TGraph graph, TVertex vertex) => graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
