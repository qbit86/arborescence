namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct EdgeIncidenceGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, SourceTargetPair<int>>,
        IGetTargetPolicy<TGraph, int, SourceTargetPair<int>>,
        IOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IIncidenceGraph<int, SourceTargetPair<int>, TEdges>
    {
        public bool TryGetTail(TGraph graph, SourceTargetPair<int> edge, out int tail) =>
            graph.TryGetTail(edge, out tail);

        public bool TryGetHead(TGraph graph, SourceTargetPair<int> edge, out int head) =>
            graph.TryGetHead(edge, out head);

        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
