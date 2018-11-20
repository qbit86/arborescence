namespace Ubiquitous
{
    public interface IGetOutEdges<in TVertex, TEdges>
    {
        bool TryGetOutEdges(TVertex vertex, out TEdges edges);
    }
}
