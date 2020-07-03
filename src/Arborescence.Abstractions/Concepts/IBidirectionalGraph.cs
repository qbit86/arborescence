namespace Arborescence
{
    public interface IBidirectionalGraph<TVertex, in TEdge, out TEdges> :
        IIncidenceGraph<TVertex, TEdge, TEdges>, IInEdgesConcept<TVertex, TEdges> { }
}
