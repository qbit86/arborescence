namespace Ubiquitous
{
    /// <summary>
    /// Represents a graph with access to vertex values.
    /// </summary>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    /// <typeparam name="TEdgeKey">The type of edge descriptors.</typeparam>
    /// <typeparam name="TVertexValue">The type of vertex values.</typeparam>
    interface IVertexGraph<TVertexKey, TEdgeKey, out TVertexValue>
    {
        TVertexValue GetVertexValue(TVertexKey vertex);
    }
}
