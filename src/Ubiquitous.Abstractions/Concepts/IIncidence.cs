namespace Ubiquitous
{
    public interface IIncidence<in TVertex, out TEdges>
    {
        TEdges EnumerateOutEdges(TVertex vertex);
    }
}
