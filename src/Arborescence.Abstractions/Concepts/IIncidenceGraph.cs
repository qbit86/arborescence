namespace Arborescence
{
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/IncidenceGraph.html

    /// <summary>
    /// Represents an incidence graph — a graph with efficient access to the out-edges of each vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IIncidenceGraph<TVertex, in TEdge, out TEdges> : IGraph<TVertex, TEdge>
    {
        /// <summary>
        /// Enumerates the out-edges of the vertex.
        /// </summary>
        /// <param name="vertex">The tail of the edges to enumerate.</param>
        /// <returns>An enumeration of out-edges of the specified vertex.</returns>
        TEdges EnumerateOutEdges(TVertex vertex);
    }
}
