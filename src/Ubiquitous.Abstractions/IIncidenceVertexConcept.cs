namespace Ubiquitous
{
    using System.Collections.Generic;

    /// <summary>
    /// Incidence concept for the type of vertex values.
    /// </summary>
    /// <typeparam name="TEdge">The type of edge descriptors.</typeparam>
    /// <typeparam name="TVertexData">The type of vertex associated data.</typeparam>
    public interface IIncidenceVertexConcept<TEdge, TVertexData>
    {
        IEnumerable<TEdge> GetOutEdges(TVertexData vertexData);
    }
}
