namespace Ubiquitous
{
    public interface IBidirectional<in TVertex, TEdges> : IIncidence<TVertex, TEdges>
    {
        bool TryGetInEdges(TVertex vertex, out TEdges edges);
    }
}
