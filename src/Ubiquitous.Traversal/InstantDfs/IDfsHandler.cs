namespace Ubiquitous.Traversal
{
    public interface IDfsHandler<in TGraph, in TVertex, in TEdge>
    {
        void StartVertex(TGraph g, TVertex v);
        void DiscoverVertex(TGraph g, TVertex v);
        void FinishVertex(TGraph g, TVertex v);
        void ExamineEdge(TGraph g, TEdge e);
        void TreeEdge(TGraph g, TEdge e);
        void FinishEdge(TGraph g, TEdge e);
    }
}
