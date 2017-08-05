namespace Ubiquitous
{
    using System.Collections.Generic;

    /// <summary>
    /// Incidence constraint for the type of vertex values.
    /// </summary>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    /// <typeparam name="TEdgeKey">The type of edge descriptors.</typeparam>
    interface IIncidenceVertexValue<TVertexKey, TEdgeKey>
    {
        IEnumerable<TEdgeKey> GetOutEdges(TVertexKey vertexKey);
    }
}
