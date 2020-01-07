namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct UncheckedEdgeBidirectionalGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, SourceTargetPair<int>>,
        IGetTargetPolicy<TGraph, int, SourceTargetPair<int>>,
        IGetOutEdgesPolicy<TGraph, int, TEdges>,
        IGetInEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IBidirectional<int, TEdges>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public bool TryGetSource(TGraph graph, SourceTargetPair<int> edge, out int source)
        {
            source = edge.Source;
            return true;
        }

        public bool TryGetTarget(TGraph graph, SourceTargetPair<int> edge, out int target)
        {
            target = edge.Target;
            return true;
        }

        public TEdges EnumerateOutEdges(TGraph graph, int vertex)
        {
            TEdges edges;
            graph.TryGetOutEdges(vertex, out edges);
            return edges;
        }

        public TEdges EnumerateInEdges(TGraph graph, int vertex)
        {
            return graph.EnumerateInEdges(vertex);
        }
    }
}
