namespace Ubiquitous
{
    /// <summary>Incidence concept for the type of vertex associated data.</summary>
    /// <typeparam name="TVertexData">The type of vertex associated data.</typeparam>
    /// <typeparam name="TEdges">The type of edges sequence.</typeparam>
    public interface IIncidenceVertexDataConcept<TVertexData, TEdges>
    {
        /// <summary>Returns outgoing edges for given vertex associated data.</summary>
        /// <param name="vertexData">Vertex associated data.</param>
        /// <returns>Outgoing edges.</returns>
        TEdges GetOutEdges(TVertexData vertexData);
    }
}
