namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
            TGraphPolicy, TColorMapPolicy, TStepPolicy>
        : IEnumerable<TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TColorMapPolicy _colorMapPolicy;

        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TGraphPolicy GraphPolicy { get; }

        private TStepPolicy StepPolicy { get; }

        internal BaselineDfsTreeStepCollection(TGraph graph, TVertex startVertex,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy, TStepPolicy stepPolicy)
        {
            Assert(colorMapPolicy != null);

            Graph = graph;
            StartVertex = startVertex;
            GraphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
            StepPolicy = stepPolicy;
        }

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
            TColorMap colorMap = _colorMapPolicy.Acquire();
            if (colorMap == null)
                yield break;

            try
            {
                yield return StepPolicy.CreateVertexStep(DfsStepKind.StartVertex, StartVertex);

                var steps = new BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                    TGraphPolicy, TColorMapPolicy, TStepPolicy>(Graph, StartVertex, colorMap,
                    GraphPolicy, _colorMapPolicy, StepPolicy);
                using (IEnumerator<TStep> stepEnumerator = steps.GetEnumerator())
                {
                    while (stepEnumerator.MoveNext())
                        yield return stepEnumerator.Current;
                }
            }
            finally
            {
                _colorMapPolicy.Release(colorMap);
            }
        }
    }
}
