namespace Ubiquitous
{
    using Traversal;

    internal sealed class DummyDfsHandler<TGraph> : IDfsHandler<TGraph, int, int>
    {
        internal int Count { get; private set; }

        public void StartVertex(TGraph g, int v) => ++Count;

        public void DiscoverVertex(TGraph g, int v) => ++Count;

        public void FinishVertex(TGraph g, int v) => ++Count;

        public void ExamineEdge(TGraph g, int e) => ++Count;

        public void TreeEdge(TGraph g, int e) => ++Count;

        public void BackEdge(TGraph g, int e) => ++Count;

        public void ForwardOrCrossEdge(TGraph g, int e) => ++Count;

        public void FinishEdge(TGraph g, int e) => ++Count;
    }
}
