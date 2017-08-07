namespace Ubiquitous
{
    using System.Collections.Generic;

    /// <summary>
    /// Incidence concept for the type of vertex values.
    /// </summary>
    /// <typeparam name="TVertexValue">The type of vertex associated data.</typeparam>
    /// <typeparam name="TEdgeKey">The type of edge descriptors.</typeparam>
    interface IIncidenceVertexValueConcept<TEdgeKey, TVertexValue>
    {
        IEnumerable<TEdgeKey> GetOutEdges(TVertexValue vertexValue);
    }
}
