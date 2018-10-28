namespace Ubiquitous
{
    public interface IGetOutEdgesPolicy<in TGraph, in TVertex, TEdges>
    {
        bool TryGetOutEdges(TGraph graph, TVertex vertex, out TEdges edges);
    }
}
