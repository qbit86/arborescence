namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineUndirectedDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep,
        TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy> : IEnumerable<TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>
        where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TVertexColorMap _vertexColorMap;
        private TEdgeColorMap _edgeColorMap;

        internal BaselineUndirectedDfsStepCollection(TGraph graph, TVertex startVertex,
            TVertexColorMap vertexColorMap, TEdgeColorMap edgeColorMap, TGraphPolicy graphPolicy,
            TVertexColorMapPolicy vertexColorMapPolicy, TEdgeColorMapPolicy edgeColorMapPolicy, TStepPolicy stepPolicy)
        {
            Assert(vertexColorMap != null);
            Assert(edgeColorMap != null);
            Assert(graphPolicy != null);
            Assert(vertexColorMapPolicy != null);
            Assert(edgeColorMapPolicy != null);
            Assert(stepPolicy != null);

            _vertexColorMap = vertexColorMap;
            _edgeColorMap = edgeColorMap;

            Graph = graph;
            StartVertex = startVertex;
            GraphPolicy = graphPolicy;
            VertexColorMapPolicy = vertexColorMapPolicy;
            EdgeColorMapPolicy = edgeColorMapPolicy;
            StepPolicy = stepPolicy;
        }

        private TGraph Graph { get; }
        private TVertex StartVertex { get; }
        private TGraphPolicy GraphPolicy { get; }
        private TVertexColorMapPolicy VertexColorMapPolicy { get; }
        private TEdgeColorMapPolicy EdgeColorMapPolicy { get; }
        private TStepPolicy StepPolicy { get; }

        public IEnumerator<TStep> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
