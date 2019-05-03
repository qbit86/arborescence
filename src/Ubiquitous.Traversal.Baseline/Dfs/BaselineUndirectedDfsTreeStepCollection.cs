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
        IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IGetInEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>, IGetSourcePolicy<TGraph, TVertex, TEdge>
        where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>, IFactory<TVertexColorMap>
        where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>, IFactory<TEdgeColorMap>
        where TStepPolicy : IUndirectedStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TEdgeColorMapPolicy _edgeColorMapPolicy;
        private TVertexColorMapPolicy _vertexColorMapPolicy;

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
            TVertexColorMap vertexColorMap = _vertexColorMapPolicy.Acquire();
            if (vertexColorMap == null)
                yield break;

            TEdgeColorMap edgeColorMap = _edgeColorMapPolicy.Acquire();
            if (edgeColorMap == null)
                yield break;

            try
            {
                yield return StepPolicy.CreateVertexStep(DfsStepKind.StartVertex, StartVertex);

                var steps = new BaselineUndirectedDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
                    TVertexColorMap, TEdgeColorMap, TStep,
                    TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>(
                    Graph, StartVertex, vertexColorMap, edgeColorMap,
                    GraphPolicy, _vertexColorMapPolicy, _edgeColorMapPolicy, StepPolicy);
                using (IEnumerator<TStep> stepEnumerator = steps.GetEnumerator())
                {
                    while (stepEnumerator.MoveNext())
                        yield return stepEnumerator.Current;
                }
            }
            finally
            {
                _edgeColorMapPolicy.Release(edgeColorMap);
                _vertexColorMapPolicy.Release(vertexColorMap);
            }
        }
    }
}
