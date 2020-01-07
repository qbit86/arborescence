namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct IndexedIncidenceGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, int>,
        IGetTargetPolicy<TGraph, int, int>,
        IGetOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IIncidenceGraph<int, int, TEdges>
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

        public TEdges EnumerateOutEdges(TGraph graph, int vertex)
        {
            TEdges edges;
            graph.TryGetOutEdges(vertex, out edges);
            return edges;
        }
    }
}
