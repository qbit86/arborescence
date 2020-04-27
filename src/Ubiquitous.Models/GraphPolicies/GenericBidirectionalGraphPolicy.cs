namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct GenericBidirectionalGraphPolicy<TGraph, TVertex, TEdge, TEdges> :
        IGetSourcePolicy<TGraph, TVertex, TEdge>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IOutEdgesPolicy<TGraph, TVertex, TEdges>,
        IInEdgesPolicy<TGraph, TVertex, TEdges>
        where TGraph : IBidirectionalGraph<TVertex, TEdge, TEdges>
    {
        public bool TryGetSource(TGraph graph, TEdge edge, out TVertex source) => graph.TryGetSource(edge, out source);

        public bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target) => graph.TryGetTarget(edge, out target);

        public TEdges EnumerateOutEdges(TGraph graph, TVertex vertex) => graph.EnumerateOutEdges(vertex);

        public TEdges EnumerateInEdges(TGraph graph, TVertex vertex) => graph.EnumerateInEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
