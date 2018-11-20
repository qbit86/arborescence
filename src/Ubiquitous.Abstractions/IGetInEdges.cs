namespace Ubiquitous
{
    public interface IGetInEdges<in TVertex, TEdges>
    {
        bool TryGetInEdges(TVertex vertex, out TEdges edges);
    }
}
