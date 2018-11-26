namespace Ubiquitous.Models
{
    public interface IGraphBuilder<out TGraph, in TVertex, TEdge>
    {
        bool TryAdd(TVertex source, TVertex target, out TEdge edge);
        TGraph ToGraph();
    }
}
