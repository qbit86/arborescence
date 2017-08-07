namespace Ubiquitous
{
    /// <summary>
    /// Represents a generic graph with defining function.
    /// </summary>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    /// <typeparam name="TEdgeKey">The type of edge descriptors.</typeparam>
    /// <typeparam name="TVertexValue">The type of vertex values.</typeparam>
    /// <typeparam name="TEdgeValue">The type of edge values.</typeparam>
    interface IGraph<TVertexKey, TEdgeKey, out TVertexValue, out TEdgeValue>
        where TEdgeValue : IEdgeValue<TVertexKey>
    {
        TVertexValue GetVertexValue(TVertexKey vertex);
        TEdgeValue GetEdgeValue(TEdgeKey edge);
    }
}
