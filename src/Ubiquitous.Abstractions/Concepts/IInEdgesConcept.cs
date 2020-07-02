namespace Ubiquitous
{
    public interface IInEdgesConcept<in TVertex, out TEdges>
    {
        TEdges EnumerateInEdges(TVertex vertex);
    }
}
