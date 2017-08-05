namespace Ubiquitous
{
    interface IVertexGraph<TVertexKey, TEdgeKey, out TVertexValue>
    {
        TVertexValue GetVertexValue(TVertexKey vertex);
    }
}
