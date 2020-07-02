namespace Arborescence
{
    public interface IHeadPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetHead(TGraph graph, TEdge edge, out TVertex head);
    }
}
