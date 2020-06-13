namespace Ubiquitous.Traversal
{
    public sealed class BfsHandler<TGraph, TVertex, TEdge> : IBfsHandler<TGraph, TVertex, TEdge>
    {
        public void OnDiscoverVertex(TGraph g, TVertex v) => DiscoverVertex?.Invoke(g, v);

        public void OnExamineVertex(TGraph g, TVertex v) => ExamineVertex?.Invoke(g, v);

        public void OnFinishVertex(TGraph g, TVertex v) => FinishVertex?.Invoke(g, v);

        public void OnExamineEdge(TGraph g, TEdge e) => ExamineEdge?.Invoke(g, e);

        public void OnTreeEdge(TGraph g, TEdge e) => TreeEdge?.Invoke(g, e);

        public void OnNonTreeGrayHeadEdge(TGraph g, TEdge e) => NonTreeGrayHeadEdge?.Invoke(g, e);

        public void OnNonTreeBlackHeadEdge(TGraph g, TEdge e) => NonTreeBlackHeadEdge?.Invoke(g, e);

        public event VertexEventHandler<TGraph, TVertex> DiscoverVertex;
        public event VertexEventHandler<TGraph, TVertex> ExamineVertex;
        public event VertexEventHandler<TGraph, TVertex> FinishVertex;
        public event EdgeEventHandler<TGraph, TEdge> ExamineEdge;
        public event EdgeEventHandler<TGraph, TEdge> TreeEdge;
        public event EdgeEventHandler<TGraph, TEdge> NonTreeGrayHeadEdge;
        public event EdgeEventHandler<TGraph, TEdge> NonTreeBlackHeadEdge;
    }
}
