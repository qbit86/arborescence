namespace Ubiquitous
{
    public interface IIncidenceGraph<TVertex, in TEdge, TEdges> :
        IGraph<TVertex, TEdge>, IIncidence<TVertex, TEdges> { }
}
