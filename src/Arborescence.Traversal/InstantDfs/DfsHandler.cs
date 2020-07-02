namespace Arborescence.Traversal
{
    public sealed class DfsHandler<TGraph, TVertex, TEdge> : IDfsHandler<TGraph, TVertex, TEdge>
    {
        public void OnStartVertex(TGraph g, TVertex v) => StartVertex?.Invoke(g, v);

        public void OnDiscoverVertex(TGraph g, TVertex v) => DiscoverVertex?.Invoke(g, v);

        public void OnFinishVertex(TGraph g, TVertex v) => FinishVertex?.Invoke(g, v);

        public void OnExamineEdge(TGraph g, TEdge e) => ExamineEdge?.Invoke(g, e);

        public void OnTreeEdge(TGraph g, TEdge e) => TreeEdge?.Invoke(g, e);

        public void OnBackEdge(TGraph g, TEdge e) => BackEdge?.Invoke(g, e);

        public void OnForwardOrCrossEdge(TGraph g, TEdge e) => ForwardOrCrossEdge?.Invoke(g, e);

        public void OnFinishEdge(TGraph g, TEdge e) => FinishEdge?.Invoke(g, e);

        public event VertexEventHandler<TGraph, TVertex> StartVertex;
        public event VertexEventHandler<TGraph, TVertex> DiscoverVertex;
        public event VertexEventHandler<TGraph, TVertex> FinishVertex;
        public event EdgeEventHandler<TGraph, TEdge> ExamineEdge;
        public event EdgeEventHandler<TGraph, TEdge> TreeEdge;
        public event EdgeEventHandler<TGraph, TEdge> BackEdge;
        public event EdgeEventHandler<TGraph, TEdge> ForwardOrCrossEdge;
        public event EdgeEventHandler<TGraph, TEdge> FinishEdge;
    }
}
