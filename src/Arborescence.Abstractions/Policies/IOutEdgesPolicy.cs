namespace Arborescence
{
    /// <summary>
    /// Defines an enumeration method for the out-edges of the vertex.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IOutEdgesPolicy<in TGraph, in TVertex, out TEdges>
    {
        /// <summary>
        /// Enumerates the out-edges of the vertex.
        /// </summary>
        /// <param name="graph">The graph to enumerate edges from.</param>
        /// <param name="vertex">The tail of the edges to enumerate.</param>
        /// <returns>An enumeration of out-edges of the specified vertex.</returns>
        TEdges EnumerateOutEdges(TGraph graph, TVertex vertex);
    }
}
