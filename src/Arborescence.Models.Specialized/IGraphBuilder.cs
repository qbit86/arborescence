namespace Arborescence.Models
{
    /// <summary>
    /// Represents a builder for graphs.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IGraphBuilder<out TGraph, in TVertex, TEdge>
    {
        /// <summary>
        /// Attempts to add the edge with the specified endpoints to the graph.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        /// <param name="edge">
        /// When this method returns, contains the added edge, if the edge was added to the graph successfully;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>A value indicating whether the edge was added successfully.</returns>
        bool TryAdd(TVertex tail, TVertex head, out TEdge edge);

        /// <summary>
        /// Creates a graph based on the contents of this builder.
        /// </summary>
        /// <returns>A graph.</returns>
        TGraph ToGraph();
    }
}
