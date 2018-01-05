namespace Ubiquitous
{
    public interface IGetTargetConcept<in TGraph, TVertex, in TEdge>
    {
        bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target);
    }
}
