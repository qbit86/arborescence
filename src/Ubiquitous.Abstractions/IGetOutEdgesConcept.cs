namespace Ubiquitous
{
    public interface IGetOutEdgesConcept<in TGraph, in TVertex, TEdges>
    {
        bool TryGetOutEdges(TGraph graph, TVertex vertex, out TEdges edges);
    }
}
