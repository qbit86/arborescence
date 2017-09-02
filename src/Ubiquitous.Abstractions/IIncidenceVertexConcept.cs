namespace Ubiquitous
{
    public interface IIncidenceVertexConcept<TGraph, TVertex, TEdges>
    {
        bool TryGetOutEdges(TGraph graph, TVertex vertex, out TEdges edges);
    }
}
