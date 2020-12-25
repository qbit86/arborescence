namespace Arborescence
{
    // https://www.boost.org/doc/libs/1_75_0/libs/graph/doc/BidirectionalGraph.html

    /// <summary>
    /// Represents a bidirectional graph — an incidence graph with efficient access to the in-edges of each vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IBidirectionalGraph<TVertex, in TEdge, out TEdges> :
        IIncidenceGraph<TVertex, TEdge, TEdges>
    {
        /// <summary>
        /// Enumerates the in-edges of the vertex.
        /// </summary>
        /// <param name="vertex">The head of the edges to enumerate.</param>
        /// <returns>An enumeration of in-edges of the specified vertex.</returns>
        TEdges EnumerateInEdges(TVertex vertex);
    }
}
