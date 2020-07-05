namespace Arborescence
{
    /// <summary>
    /// Represents an incidence graph — a graph with efficient access to the out-edges of each vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IIncidenceGraph<TVertex, in TEdge, out TEdges> :
        IGraph<TVertex, TEdge>, IOutEdgesConcept<TVertex, TEdges> { }
}
