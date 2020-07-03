namespace Arborescence
{
    public interface IInEdgesConcept<in TVertex, out TEdges>
    {
        TEdges EnumerateInEdges(TVertex vertex);
    }
}
