namespace Ubiquitous
{
    public interface IGetOutEdgesPolicy<in TGraph, in TVertex, TEdges>
    {
        void TryGetOutEdges(TGraph graph, TVertex vertex, out TEdges edges);
    }
}
