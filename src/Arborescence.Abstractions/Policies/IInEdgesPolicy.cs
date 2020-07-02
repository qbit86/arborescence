namespace Arborescence
{
    public interface IInEdgesPolicy<in TGraph, in TVertex, out TEdges>
    {
        TEdges EnumerateInEdges(TGraph graph, TVertex vertex);
    }
}
