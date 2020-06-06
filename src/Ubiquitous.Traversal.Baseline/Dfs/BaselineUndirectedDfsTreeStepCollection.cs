// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal sealed class BaselineUndirectedDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep,
        TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy> : IEnumerable<TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy :
        IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IInEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetHeadPolicy<TGraph, TVertex, TEdge>, IGetTailPolicy<TGraph, TVertex, TEdge>
        where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>
        where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>
        where TStepPolicy : IUndirectedStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TEdgeColorMapPolicy _edgeColorMapPolicy;
        private TVertexColorMapPolicy _vertexColorMapPolicy;

        internal BaselineUndirectedDfsTreeStepCollection(TGraph graph, TVertex startVertex,
            TVertexColorMap vertexColorMap, TEdgeColorMap edgeColorMap,
            TGraphPolicy graphPolicy,
            TVertexColorMapPolicy vertexColorMapPolicy, TEdgeColorMapPolicy edgeColorMapPolicy,
            TStepPolicy stepPolicy)
        {
            Assert(graphPolicy != null);
            Assert(vertexColorMapPolicy != null);
            Assert(edgeColorMapPolicy != null);
            Assert(stepPolicy != null);

            Graph = graph;
            StartVertex = startVertex;
            VertexColorMap = vertexColorMap;
            EdgeColorMap = edgeColorMap;
            GraphPolicy = graphPolicy;
            StepPolicy = stepPolicy;

            _vertexColorMapPolicy = vertexColorMapPolicy;
            _edgeColorMapPolicy = edgeColorMapPolicy;
        }

        private TGraph Graph { get; }
        private TVertex StartVertex { get; }
        private TVertexColorMap VertexColorMap { get; }
        private TEdgeColorMap EdgeColorMap { get; }
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
            yield return StepPolicy.CreateVertexStep(DfsStepKind.StartVertex, StartVertex);

            var steps = new BaselineUndirectedDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
                TVertexColorMap, TEdgeColorMap, TStep,
                TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>(
                Graph, StartVertex, VertexColorMap, EdgeColorMap,
                GraphPolicy, _vertexColorMapPolicy, _edgeColorMapPolicy, StepPolicy);
            using (IEnumerator<TStep> stepEnumerator = steps.GetEnumerator())
            {
                while (stepEnumerator.MoveNext())
                    yield return stepEnumerator.Current;
            }
        }
    }
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
