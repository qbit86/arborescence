namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using Traversal;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    internal readonly struct IndexedDfsEdgeHandler<TGraph> : IDfsHandler<TGraph, int, int>
    {
        private readonly IList<DfsStep<int>> _steps;

        public IndexedDfsEdgeHandler(IList<DfsStep<int>> steps)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        }

        public void OnStartVertex(TGraph g, int v) { }

        public void OnDiscoverVertex(TGraph g, int v) { }

        public void OnFinishVertex(TGraph g, int v) { }

        public void OnExamineEdge(TGraph g, int e) => _steps.Add(DfsStep.Create(DfsStepKind.ExamineEdge, e));

        public void OnTreeEdge(TGraph g, int e) => _steps.Add(DfsStep.Create(DfsStepKind.TreeEdge, e));

        public void OnBackEdge(TGraph g, int e) => _steps.Add(DfsStep.Create(DfsStepKind.BackEdge, e));

        public void OnForwardOrCrossEdge(TGraph g, int e) =>
            _steps.Add(DfsStep.Create(DfsStepKind.ForwardOrCrossEdge, e));

        public void OnFinishEdge(TGraph g, int e) => _steps.Add(DfsStep.Create(DfsStepKind.FinishEdge, e));
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
