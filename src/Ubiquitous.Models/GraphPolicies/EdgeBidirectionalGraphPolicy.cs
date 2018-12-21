namespace Ubiquitous.Models
{
    public readonly struct EdgeBidirectionalGraphPolicy<TGraph, TEdges> :
        IGetSourcePolicy<TGraph, int, SourceTargetPair<int>>,
        IGetTargetPolicy<TGraph, int, SourceTargetPair<int>>,
        IGetOutEdgesPolicy<TGraph, int, TEdges>,
        IGetInEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IBidirectionalGraph<int, SourceTargetPair<int>, TEdges>
    {
        public bool TryGetSource(TGraph graph, SourceTargetPair<int> edge, out int source)
        {
            return graph.TryGetSource(edge, out source);
        }

        public bool TryGetTarget(TGraph graph, SourceTargetPair<int> edge, out int target)
        {
            return graph.TryGetTarget(edge, out target);
        }

        public bool TryGetOutEdges(TGraph graph, int vertex, out TEdges edges)
        {
            return graph.TryGetOutEdges(vertex, out edges);
        }

        public bool TryGetInEdges(TGraph graph, int vertex, out TEdges edges)
        {
            return graph.TryGetInEdges(vertex, out edges);
        }
    }
}
