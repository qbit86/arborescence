namespace Ubiquitous
{
    public interface IBidirectional<in TVertex, TEdges> : IIncidence<TVertex, TEdges>
    {
        void TryGetInEdges(TVertex vertex, out TEdges edges);
    }
}
