namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal sealed class BaselineUndirectedDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep,
        TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy> : IEnumerable<TStep>
    {
        private TVertexColorMapPolicy _vertexColorMapPolicy;
        private TEdgeColorMapPolicy _edgeColorMapPolicy;

        internal BaselineUndirectedDfsTreeStepCollection(TGraph graph, TVertex startVertex, TGraphPolicy graphPolicy,
            TVertexColorMapPolicy vertexColorMapPolicy, TEdgeColorMapPolicy edgeColorMapPolicy,
            TStepPolicy stepPolicy)
        {
            Assert(graphPolicy != null);
            Assert(vertexColorMapPolicy != null);
            Assert(edgeColorMapPolicy != null);
            Assert(stepPolicy != null);

            Graph = graph;
            StartVertex = startVertex;
            GraphPolicy = graphPolicy;
            StepPolicy = stepPolicy;

            _vertexColorMapPolicy = vertexColorMapPolicy;
            _edgeColorMapPolicy = edgeColorMapPolicy;
        }

        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TGraphPolicy GraphPolicy { get; }

        private TStepPolicy StepPolicy { get; }

        public IEnumerator<TStep> GetEnumerator()
        {
            return GetEnumeratorCoroutine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<TStep> result = GetEnumerator();
            return result;
        }

        private IEnumerator<TStep> GetEnumeratorCoroutine()
        {
            throw new NotImplementedException();
        }
    }
}
