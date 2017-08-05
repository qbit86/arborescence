namespace Ubiquitous
{
    interface IGraph<TVertexKey, TEdgeKey, out TEdgeValue>
        where TEdgeValue : IEdgeValue<TVertexKey>
    {
        TEdgeValue GetEdgeValue(TEdgeKey edge);
    }
}
