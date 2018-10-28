namespace Ubiquitous
{
    public interface IGetTargetPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetTarget(TGraph graph, TEdge edge, out TVertex target);
    }
}
