namespace Ubiquitous
{
    public interface IGetTargetPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetHead(TGraph graph, TEdge edge, out TVertex head);
    }
}
