namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Traversal;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct DfsHandler<TGraph> : IDfsHandler<TGraph, int, int>
    {
        private readonly IList<IndexedDfsStep> _steps;

        public DfsHandler(IList<IndexedDfsStep> steps)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        }

        public void StartVertex(TGraph g, int v) => _steps.Add(new IndexedDfsStep(DfsStepKind.StartVertex, v));

        public void DiscoverVertex(TGraph g, int v) => _steps.Add(new IndexedDfsStep(DfsStepKind.DiscoverVertex, v));

        public void FinishVertex(TGraph g, int v) => _steps.Add(new IndexedDfsStep(DfsStepKind.FinishVertex, v));

        public void ExamineEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.ExamineEdge, e));

        public void TreeEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.TreeEdge, e));

        public void BackEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.BackEdge, e));

        public void ForwardOrCrossEdge(TGraph g, int e) =>
            _steps.Add(new IndexedDfsStep(DfsStepKind.ForwardOrCrossEdge, e));

        public void FinishEdge(TGraph g, int e) => _steps.Add(new IndexedDfsStep(DfsStepKind.FinishEdge, e));
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
