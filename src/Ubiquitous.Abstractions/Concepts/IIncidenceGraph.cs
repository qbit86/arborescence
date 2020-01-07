namespace Ubiquitous
{
    public interface IIncidenceGraph<TVertex, in TEdge, out TEdges> :
        IGraph<TVertex, TEdge>, IIncidence<TVertex, TEdges> { }
}
