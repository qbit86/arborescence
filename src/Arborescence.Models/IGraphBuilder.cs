namespace Arborescence.Models
{
    public interface IGraphBuilder<out TGraph, in TVertex, TEdge>
    {
        bool TryAdd(TVertex tail, TVertex head, out TEdge edge);
        TGraph ToGraph();
    }
}
