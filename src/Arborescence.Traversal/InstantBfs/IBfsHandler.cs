namespace Arborescence.Traversal
{
    // https://www.boost.org/doc/libs/1_73_0/libs/graph/doc/BFSVisitor.html

    /// <summary>
    /// Defines callbacks to be invoked while traversing a graph in a BFS manner.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
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
