namespace Ubiquitous
{
    public interface IBidirectional<in TVertex, out TEdges> : IIncidence<TVertex, TEdges>
    {
        TEdges EnumerateInEdges(TVertex vertex);
    }
}
