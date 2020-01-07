namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct EdgeIncidenceGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, SourceTargetPair<int>>,
        IGetTargetPolicy<TGraph, int, SourceTargetPair<int>>,
        IGetOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IIncidenceGraph<int, SourceTargetPair<int>, TEdges>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public bool TryGetSource(TGraph graph, SourceTargetPair<int> edge, out int source)
        {
            return graph.TryGetSource(edge, out source);
        }

        public bool TryGetTarget(TGraph graph, SourceTargetPair<int> edge, out int target)
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
