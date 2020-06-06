namespace Ubiquitous
{
    public interface IGetTailPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetTail(TGraph graph, TEdge edge, out TVertex tail);
    }
}
