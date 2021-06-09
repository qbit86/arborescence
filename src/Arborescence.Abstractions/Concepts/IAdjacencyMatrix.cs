namespace Arborescence
{
    using System.Diagnostics.CodeAnalysis;

    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AdjacencyMatrix.html

    /// <summary>
    /// Provides an interface for efficient access to any edge in the graph given its endpoints.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IAdjacencyMatrix<in TVertex, TEdge>
    {
        /// <summary>
        /// Gets the edge associated with the specified tail and head.
        /// </summary>
        /// <param name="tail">The tail of the edge to get.</param>
        /// <param name="head">The head of the edge to get.</param>
        /// <param name="edge">
        /// When this method returns, contains the edge with the specified endpoints, if the edge is found;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>A value indicating whether the edge was found successfully.</returns>
        bool TryGetEdge(TVertex tail, TVertex head, [MaybeNullWhen(false)] out TEdge edge);
    }
}
