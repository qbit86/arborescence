namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsForestStepCollection<TGraph, TVertex, TEdge,
            TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TStep, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
        : IEnumerable<TStep>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TColorMapPolicy _colorMapPolicy;

        private TGraph Graph { get; }

        private TVertexEnumerable VertexCollection { get; }

        private TGraphPolicy GraphPolicy { get; }

        private TVertexEnumerablePolicy VertexEnumerablePolicy { get; }

        private TStepPolicy StepPolicy { get; }

        internal BaselineDfsForestStepCollection(TGraph graph, TVertexEnumerable vertexCollection,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TVertexEnumerablePolicy vertexEnumerablePolicy, TStepPolicy stepPolicy)
        {
            Assert(vertexCollection != null);
            Assert(colorMapPolicy != null);

            Graph = graph;
            VertexCollection = vertexCollection;
            GraphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
            VertexEnumerablePolicy = vertexEnumerablePolicy;
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

            TVertexEnumerator vertexEnumerator = VertexEnumerablePolicy.GetEnumerator(VertexCollection);
            try
            {
                if (vertexEnumerator == null)
                    yield break;

                while (vertexEnumerator.MoveNext())
                {
                    TVertex vertex = vertexEnumerator.Current;

                    if (!_colorMapPolicy.TryGet(colorMap, vertex, out Color vertexColor))
                        vertexColor = Color.None;

                    if (vertexColor != Color.None && vertexColor != Color.White)
                        continue;

                    yield return StepPolicy.CreateVertexStep(DfsStepKind.StartVertex, vertex);

                    var steps = new BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                        TStep, TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                        Graph, vertex, colorMap, GraphPolicy, _colorMapPolicy, StepPolicy);
                    using (IEnumerator<TStep> stepEnumerator = steps.GetEnumerator())
                    {
                        while (stepEnumerator.MoveNext())
                            yield return stepEnumerator.Current;
                    }
                }
            }
            finally
            {
                vertexEnumerator?.Dispose();
                _colorMapPolicy.Release(colorMap);
            }
        }
    }
}
