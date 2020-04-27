namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedIncidenceGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, int>,
        IGetTargetPolicy<TGraph, int, int>,
        IOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IIncidenceGraph<int, int, TEdges>
    {
        public bool TryGetSource(TGraph graph, int edge, out int source) => graph.TryGetSource(edge, out source);

        public bool TryGetTarget(TGraph graph, int edge, out int target) => graph.TryGetTarget(edge, out target);

        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);
#pragma warning restore CA1815 // Override equals and operator equals on value types
    }
}
