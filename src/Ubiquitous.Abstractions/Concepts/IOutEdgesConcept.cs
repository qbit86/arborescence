namespace Ubiquitous
{
    public interface IOutEdgesConcept<in TVertex, out TEdges>
    {
        TEdges EnumerateOutEdges(TVertex vertex);
    }
}
