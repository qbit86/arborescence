namespace Ubiquitous.Models
{
    public readonly struct GenericIncidenceGraphPolicy<TGraph, TVertex, TEdge, TEdges> :
        IGetSourcePolicy<TGraph, TVertex, TEdge>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IGetOutEdgesPolicy<TGraph, TVertex, TEdges>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdges>
    {
        public bool TryGetSource(TGraph graph, TEdge edge, out TVertex source)
        {
            return graph.TryGetSource(edge, out source);
        }

        public bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target)
        {
            return graph.TryGetTarget(edge, out target);
        }

        public bool TryGetOutEdges(TGraph graph, TVertex vertex, out TEdges edges)
        {
            return graph.TryGetOutEdges(vertex, out edges);
        }
    }
}
