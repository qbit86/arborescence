namespace Arborescence
{
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AdjacencyGraph.html

    /// <summary>
    /// Provides an interface for efficient access of the adjacent vertices to a vertex in a graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TVertices">The type of the vertices enumerator.</typeparam>
    public interface IAdjacency<in TVertex, out TVertices>
    {
        /// <summary>
        /// Enumerates the adjacent vertices of the vertex.
        /// </summary>
        /// <param name="vertex">The vertex which neighbours to enumerate.</param>
        /// <returns>An enumeration of adjacent vertices of the specified vertex.</returns>
        TVertices EnumerateAdjacentVertices(TVertex vertex);
    }
}
