namespace Arborescence.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct GenericBidirectionalGraphPolicy<TGraph, TVertex, TEdge, TEdges> :
        ITailPolicy<TGraph, TVertex, TEdge>,
        IHeadPolicy<TGraph, TVertex, TEdge>,
        IOutEdgesPolicy<TGraph, TVertex, TEdges>,
        IInEdgesPolicy<TGraph, TVertex, TEdges>
        where TGraph : IBidirectionalGraph<TVertex, TEdge, TEdges>
    {
        /// <inheritdoc/>
        public bool TryGetTail(TGraph graph, TEdge edge, out TVertex tail) => graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(TGraph graph, TEdge edge, out TVertex head) => graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public TEdges EnumerateOutEdges(TGraph graph, TVertex vertex) => graph.EnumerateOutEdges(vertex);

        /// <inheritdoc/>
        public TEdges EnumerateInEdges(TGraph graph, TVertex vertex) => graph.EnumerateInEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
