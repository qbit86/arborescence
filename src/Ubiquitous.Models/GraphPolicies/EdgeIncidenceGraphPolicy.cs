namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct EdgeIncidenceGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, SourceTargetPair<int>>,
        IGetTargetPolicy<TGraph, int, SourceTargetPair<int>>,
        IOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IIncidenceGraph<int, SourceTargetPair<int>, TEdges>
    {
        public bool TryGetSource(TGraph graph, SourceTargetPair<int> edge, out int source) =>
            graph.TryGetSource(edge, out source);

        public bool TryGetTarget(TGraph graph, SourceTargetPair<int> edge, out int target) =>
            graph.TryGetTarget(edge, out target);

        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
