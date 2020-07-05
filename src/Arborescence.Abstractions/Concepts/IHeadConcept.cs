namespace Arborescence
{
    /// <summary>
    /// Represents an incidence function mapping the edge to its head.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IHeadConcept<TVertex, in TEdge>
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
        bool TryGetHead(TEdge edge, out TVertex head);
    }
}
