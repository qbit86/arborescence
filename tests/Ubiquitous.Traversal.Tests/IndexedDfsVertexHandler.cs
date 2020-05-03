namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Traversal;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    internal readonly struct IndexedDfsVertexHandler<TGraph> : IDfsHandler<TGraph, int, int>
    {
        private readonly IList<DfsStep<int>> _steps;

        public IndexedDfsVertexHandler(IList<DfsStep<int>> steps)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        }

        public void OnStartVertex(TGraph g, int v) => _steps.Add(DfsStep.Create(DfsStepKind.StartVertex, v));

        public void OnDiscoverVertex(TGraph g, int v) =>
            _steps.Add(DfsStep.Create(DfsStepKind.DiscoverVertex, v));

        public void OnFinishVertex(TGraph g, int v) => _steps.Add(DfsStep.Create(DfsStepKind.FinishVertex, v));

        public void OnExamineEdge(TGraph g, int e) { }

        public void OnTreeEdge(TGraph g, int e) { }

        public void OnBackEdge(TGraph g, int e) { }

        public void OnForwardOrCrossEdge(TGraph g, int e) { }

        public void OnFinishEdge(TGraph g, int e) { }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
