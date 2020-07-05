namespace Arborescence
{
    /// <summary>
    /// Defines an incidence function mapping the edge to its tail.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface ITailPolicy<in TGraph, TVertex, in TEdge>
    {
        /// <summary>
        /// Gets the vertex associated with the specified edge as its tail.
        /// </summary>
        /// <param name="graph">The graph to get from.</param>
        /// <param name="edge">The out-edge of the vertex to get.</param>
        /// <param name="tail">
        /// When this method returns, contains the tail of the specified edge, if the edge is found;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>A value indicating whether the edge was found successfully.</returns>
        bool TryGetTail(TGraph graph, TEdge edge, out TVertex tail);
    }
}
