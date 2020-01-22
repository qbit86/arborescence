namespace Ubiquitous.Traversal
{
    public interface IDfsHandler<in TGraph, in TVertex, in TEdge>
    {
        void StartVertex(TGraph g, TVertex v);
        void DiscoverVertex(TGraph g, TVertex v);
        void FinishEdge(TGraph g, TEdge e);
    }
}
