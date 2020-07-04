namespace Arborescence
{
    /// <summary>
    /// Provides an enumerator for the in-edges of the vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IInEdgesConcept<in TVertex, out TEdges>
    {
        /// <summary>
        /// Enumerates the in-edges of the vertex.
        /// </summary>
        /// <param name="vertex">The head of the edges to enumerate.</param>
        /// <returns>An enumeration of in-edges of the specified vertex.</returns>
        TEdges EnumerateInEdges(TVertex vertex);
    }
}
