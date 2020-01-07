namespace Ubiquitous
{
    public interface IIncidence<in TVertex, TEdges>
    {
        void TryGetOutEdges(TVertex vertex, out TEdges edges);
    }
}
