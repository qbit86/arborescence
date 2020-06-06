namespace Ubiquitous
{
    public interface IGetHeadPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetHead(TGraph graph, TEdge edge, out TVertex head);
    }
}
