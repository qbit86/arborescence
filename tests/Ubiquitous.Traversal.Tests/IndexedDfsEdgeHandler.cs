namespace Ubiquitous
{
    using System;
    using Traversal;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    internal readonly struct IndexedDfsEdgeHandler<TGraph> : IDfsHandler<TGraph, int, int>
    {
        public void OnStartVertex(TGraph g, int v) { }

        public void OnDiscoverVertex(TGraph g, int v) { }

        public void OnFinishVertex(TGraph g, int v) { }

        public void OnExamineEdge(TGraph g, int e) => throw new NotImplementedException();

        public void OnTreeEdge(TGraph g, int e) => throw new NotImplementedException();

        public void OnBackEdge(TGraph g, int e) => throw new NotImplementedException();

        public void OnForwardOrCrossEdge(TGraph g, int e) => throw new NotImplementedException();

        public void OnFinishEdge(TGraph g, int e) => throw new NotImplementedException();
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
