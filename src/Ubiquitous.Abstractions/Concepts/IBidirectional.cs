namespace Ubiquitous
{
    public interface IBidirectional<in TVertex, TEdges> : IIncidence<TVertex, TEdges>
    {
        TEdges EnumerateInEdges(TVertex vertex);
    }
}
