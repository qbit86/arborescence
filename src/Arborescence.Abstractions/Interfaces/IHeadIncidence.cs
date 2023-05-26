namespace Arborescence
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a notion of incidence — an access to the head of each edge.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Incidence_(graph)"/>
    /// <seealso href="https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_with_own_identity)"/>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IHeadIncidence<TVertex, in TEdge>
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
