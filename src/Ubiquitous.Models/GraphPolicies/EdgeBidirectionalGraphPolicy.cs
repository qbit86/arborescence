namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct EdgeBidirectionalGraphPolicy<TGraph, TEdges> :
        IGetTailPolicy<TGraph, int, SourceTargetPair<int>>,
        IGetHeadPolicy<TGraph, int, SourceTargetPair<int>>,
        IOutEdgesPolicy<TGraph, int, TEdges>,
        IInEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IBidirectionalGraph<int, SourceTargetPair<int>, TEdges>
    {
        public bool TryGetTail(TGraph graph, SourceTargetPair<int> edge, out int tail) =>
            graph.TryGetTail(edge, out tail);

        public bool TryGetHead(TGraph graph, SourceTargetPair<int> edge, out int head) =>
            graph.TryGetHead(edge, out head);

        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);

        public TEdges EnumerateInEdges(TGraph graph, int vertex) => graph.EnumerateInEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
