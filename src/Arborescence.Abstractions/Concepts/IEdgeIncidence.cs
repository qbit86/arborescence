namespace Arborescence
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents an incidence concept — an access to the head of each edge.
    /// </summary>
    public interface IEdgeIncidence<TVertex, in TEdge>
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
    }
}
