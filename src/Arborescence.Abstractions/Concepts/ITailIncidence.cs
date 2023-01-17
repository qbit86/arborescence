namespace Arborescence
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents an incidence concept — an access to the tail of each edge.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Incidence_(graph)"/>
    /// <seealso href="https://en.wikipedia.org/wiki/Multigraph#Directed_multigraph_(edges_with_own_identity)"/>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface ITailIncidence<TVertex, in TEdge>
    {
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
