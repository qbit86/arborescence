namespace Ubiquitous
{
    public interface IGetSourcePolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetSource(TGraph graph, TEdge edge, out TVertex source);
    }
}
