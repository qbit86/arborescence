namespace Ubiquitous
{
    public interface ITailPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetTail(TGraph graph, TEdge edge, out TVertex tail);
    }
}
