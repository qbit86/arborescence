namespace Ubiquitous
{
    public interface IGetInEdgesPolicy<in TGraph, in TVertex, TEdges>
    {
        bool TryGetInEdges(TGraph graph, TVertex vertex, out TEdges edges);
    }
}
