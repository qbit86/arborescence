namespace Arborescence
{
    using Traversal;

#pragma warning disable CA1801 // Review unused parameters
    internal sealed class DummyHandler<TGraph> : IDfsHandler<TGraph, int, int>, IBfsHandler<TGraph, int, int>
    {
        internal int Count { get; private set; }

        public void OnExamineVertex(TGraph g, int v) => ++Count;

        public void OnNonTreeGrayHeadEdge(TGraph g, int e) => ++Count;

        public void OnNonTreeBlackHeadEdge(TGraph g, int e) => ++Count;

        public void OnStartVertex(TGraph g, int v) => ++Count;

        public void OnDiscoverVertex(TGraph g, int v) => ++Count;

        public void OnFinishVertex(TGraph g, int v) => ++Count;

        public void OnExamineEdge(TGraph g, int e) => ++Count;

        public void OnTreeEdge(TGraph g, int e) => ++Count;

        public void OnBackEdge(TGraph g, int e) => ++Count;

        public void OnForwardOrCrossEdge(TGraph g, int e) => ++Count;

        public void OnFinishEdge(TGraph g, int e) => ++Count;

        public void Reset() => Count = 0;
    }
#pragma warning restore CA1801
}
