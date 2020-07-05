namespace Arborescence
{
    /// <summary>
    /// Represents a bidirectional graph — an incidence graph with efficient access to the in-edges of each vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IBidirectionalGraph<TVertex, in TEdge, out TEdges> :
        IIncidenceGraph<TVertex, TEdge, TEdges>, IInEdgesConcept<TVertex, TEdges> { }
}
