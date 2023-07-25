namespace Arborescence
{
    // https://boost.org/doc/libs/1_82_0/libs/graph/doc/AdjacencyGraph.html

    /// <summary>
    /// Provides an interface for efficient access to the adjacent vertices of a vertex in a graph.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Glossary_of_graph_theory#adjacent"/>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighbors">The type of the enumerator for the neighbors.</typeparam>
    public interface IOutNeighborsAdjacency<in TVertex, out TNeighbors>
    {
        /// <summary>
        /// Enumerates the out-neighbors of the vertex.
        /// </summary>
        /// <param name="vertex">The vertex which out-neighbours to enumerate.</param>
        /// <returns>An enumeration of out-neighbors of the specified vertex.</returns>
        TNeighbors EnumerateOutNeighbors(TVertex vertex);
    }
}
