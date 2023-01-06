namespace Arborescence
{
    public interface IVertexIncidence<in TVertex, out TEdges>
    {
        TEdges EnumerateOutEdges(TVertex vertex);
    }
}
