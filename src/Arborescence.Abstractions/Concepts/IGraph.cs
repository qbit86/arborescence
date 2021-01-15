namespace Arborescence
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a graph as a pair of incidence functions.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IGraph<TVertex, in TEdge>
    {
        /// <summary>
        /// Gets the vertex associated with the specified edge as its head.
        /// </summary>
        /// <param name="edge">The in-edge of the vertex to get.</param>
        /// <param name="head">
        /// When this method returns, contains the head of the specified edge, if the edge is found;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>A value indicating whether the edge was found successfully.</returns>
        bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head);

        /// <summary>
        /// Gets the vertex associated with the specified edge as its tail.
        /// </summary>
        /// <param name="edge">The out-edge of the vertex to get.</param>
        /// <param name="tail">
        /// When this method returns, contains the tail of the specified edge, if the edge is found;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>A value indicating whether the edge was found successfully.</returns>
        bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail);
    }
}
