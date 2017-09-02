namespace Ubiquitous
{
    public interface IEdgeConcept<TGraph, TVertex, TEdge>
    {
        bool TryGetSource(TGraph graph, TEdge edge, out TVertex source);
        bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target);
    }
}
