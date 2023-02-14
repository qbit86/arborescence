namespace Arborescence
{
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AdjacencyGraph.html

    /// <summary>
    /// Provides an interface for efficient access to the adjacent vertices of a vertex in a graph.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Glossary_of_graph_theory#adjacent"/>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TVertices">The type of the vertices enumerator.</typeparam>
    public interface IAdjacency<in TVertex, out TVertices>
    {
        /// <summary>
        /// Enumerates the out-neighbors of the vertex.
        /// </summary>
        /// <param name="vertex">The vertex which out-neighbours to enumerate.</param>
        /// <returns>An enumeration of out-neighbors of the specified vertex.</returns>
        TVertices EnumerateOutNeighbors(TVertex vertex);
    }
}
