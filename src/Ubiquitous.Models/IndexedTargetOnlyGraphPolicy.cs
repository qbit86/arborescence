namespace Ubiquitous
{
    public readonly struct IndexedTargetOnlyGraphPolicy<TGraph, TEdges> :
        IGetTargetPolicy<TGraph, int, int>,
        IGetOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IGetTarget<int, int>, IGetOutEdges<int, TEdges>
    {
        public bool TryGetTarget(TGraph graph, int edge, out int target)
        {
            return graph.TryGetTarget(edge, out target);
        }

        public bool TryGetOutEdges(TGraph graph, int vertex, out TEdges edges)
        {
            return graph.TryGetOutEdges(vertex, out edges);
        }
    }
}
