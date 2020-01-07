namespace Ubiquitous
{
    public interface IGetInEdgesPolicy<in TGraph, in TVertex, TEdges>
    {
        void TryGetInEdges(TGraph graph, TVertex vertex, out TEdges edges);
    }
}
