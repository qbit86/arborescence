namespace Ubiquitous
{
    public interface IOutEdgesPolicy<in TGraph, in TVertex, out TEdges>
    {
        TEdges EnumerateOutEdges(TGraph graph, TVertex vertex);
    }
}
