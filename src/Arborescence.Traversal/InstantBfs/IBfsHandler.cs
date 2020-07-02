namespace Arborescence.Traversal
{
    // https://www.boost.org/doc/libs/1_73_0/libs/graph/doc/BFSVisitor.html

    public interface IBfsHandler<in TGraph, in TVertex, in TEdge>
    {
        void OnDiscoverVertex(TGraph g, TVertex v);
        void OnExamineVertex(TGraph g, TVertex v);
        void OnFinishVertex(TGraph g, TVertex v);
        void OnExamineEdge(TGraph g, TEdge e);
        void OnTreeEdge(TGraph g, TEdge e);
        void OnNonTreeGrayHeadEdge(TGraph g, TEdge e);
        void OnNonTreeBlackHeadEdge(TGraph g, TEdge e);
    }
}
