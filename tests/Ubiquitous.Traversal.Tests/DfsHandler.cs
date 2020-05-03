namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Traversal;
    using IndexedDfsStep = Traversal.DfsStep<int>;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    internal readonly struct DfsHandler<TGraph> : IDfsHandler<TGraph, int, int>
    {
        private readonly IList<IndexedDfsStep> _steps;

        public DfsHandler(IList<IndexedDfsStep> steps)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        }

        public void OnStartVertex(TGraph g, int v) => _steps.Add(new IndexedDfsStep(DfsStepKind.StartVertex, v));

        public void OnDiscoverVertex(TGraph g, int v) => _steps.Add(new IndexedDfsStep(DfsStepKind.DiscoverVertex, v));

        public void OnFinishVertex(TGraph g, int v) => _steps.Add(new IndexedDfsStep(DfsStepKind.FinishVertex, v));

        public void OnExamineEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.ExamineEdge, e));

        public void OnTreeEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.TreeEdge, e));

        public void OnBackEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.BackEdge, e));

        public void OnForwardOrCrossEdge(TGraph g, int e) =>
            _steps.Add(new IndexedDfsStep(DfsStepKind.ForwardOrCrossEdge, e));

        public void OnFinishEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.FinishEdge, e));
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
