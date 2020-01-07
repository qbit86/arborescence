namespace Ubiquitous
{
    public interface IGetInEdgesPolicy<in TGraph, in TVertex, out TEdges>
    {
        TEdges EnumerateInEdges(TGraph graph, TVertex vertex);
    }
}
