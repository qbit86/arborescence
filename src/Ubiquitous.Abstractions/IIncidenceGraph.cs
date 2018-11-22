namespace Ubiquitous
{
    public interface IIncidenceGraph<TVertex, in TEdge, TEdges> : IGraph<TVertex, TEdge>
    {
        bool TryGetOutEdges(TVertex vertex, out TEdges edges);
    }
}
