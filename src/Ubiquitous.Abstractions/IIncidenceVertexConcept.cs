namespace Ubiquitous
{
    using System.Collections.Generic;

    /// <summary>
    /// Incidence concept for the type of vertex values.
    /// </summary>
    /// <typeparam name="TEdgeKey">The type of edge descriptors.</typeparam>
    /// <typeparam name="TVertexData">The type of vertex associated data.</typeparam>
    public interface IIncidenceVertexConcept<TEdgeKey, TVertexData>
    {
        IEnumerable<TEdgeKey> GetOutEdges(TVertexData vertexData);
    }
}
