namespace Arborescence
{
    public interface IIncidenceGraph<TVertex, in TEdge, out TEdges> :
        IGraph<TVertex, TEdge>, IOutEdgesConcept<TVertex, TEdges> { }
}
