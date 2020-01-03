namespace Ubiquitous
{
    public interface IIncidence<in TVertex, TEdges>
    {
        bool TryGetOutEdges(TVertex vertex, out TEdges edges);
    }
}
