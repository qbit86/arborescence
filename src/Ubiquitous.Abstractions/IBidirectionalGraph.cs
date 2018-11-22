namespace Ubiquitous
{
    public interface IBidirectionalGraph<TVertex, in TEdge, TEdges> : IIncidenceGraph<TVertex, TEdge, TEdges>
    {
        bool TryGetInEdges(TVertex vertex, out TEdges edges);
    }
}
