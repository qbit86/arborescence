namespace Ubiquitous
{
    public interface IGetSourcePolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetTail(TGraph graph, TEdge edge, out TVertex source);
    }
}
