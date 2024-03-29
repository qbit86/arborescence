namespace Arborescence
{
    /// <summary>
    /// Represents a notion of incidence — an efficient access to the out-edges of each vertex.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Incidence_(graph)"/>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdges">The type of the enumerator for the edges.</typeparam>
    public interface IOutEdgesIncidence<in TVertex, out TEdges>
    {
        /// <summary>
        /// Enumerates the out-edges of the vertex.
        /// </summary>
        /// <param name="vertex">The tail of the edges to enumerate.</param>
        /// <returns>An enumeration of out-edges of the specified vertex.</returns>
        TEdges EnumerateOutEdges(TVertex vertex);
    }
}
