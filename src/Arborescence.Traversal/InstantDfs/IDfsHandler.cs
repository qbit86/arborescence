namespace Arborescence.Traversal
{
    // https://www.boost.org/doc/libs/1_73_0/libs/graph/doc/DFSVisitor.html

    public interface IDfsHandler<in TGraph, in TVertex, in TEdge>
    {
        void OnStartVertex(TGraph g, TVertex v);
        void OnDiscoverVertex(TGraph g, TVertex v);
        void OnFinishVertex(TGraph g, TVertex v);
        void OnExamineEdge(TGraph g, TEdge e);
        void OnTreeEdge(TGraph g, TEdge e);
        void OnBackEdge(TGraph g, TEdge e);
        void OnForwardOrCrossEdge(TGraph g, TEdge e);
        void OnFinishEdge(TGraph g, TEdge e);
    }
}
