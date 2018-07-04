namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TGraphConcept, TColorMapFactory>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TVertexEnumerator _vertexEnumerator;

        private TColorMapFactory _colorMapFactory;

        private TGraph Graph { get; }

        private TGraphConcept GraphConcept { get; }

        internal BaselineDfsForestStepCollection(TGraph graph, TVertexEnumerator vertexEnumerator,
            TGraphConcept graphConcept, TColorMapFactory colorMapFactory)
        {
            Assert(vertexEnumerator != null);
            Assert(colorMapFactory != null);

            _vertexEnumerator = vertexEnumerator;

            Graph = graph;
            GraphConcept = graphConcept;
            _colorMapFactory = colorMapFactory;
        }

        public IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumerator()
        {
            return GetEnumeratorCoroutine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<Step<DfsStepKind, TVertex, TEdge>> result = GetEnumerator();
            return result;
        }

        private IEnumerator<Step<DfsStepKind, TVertex, TEdge>> GetEnumeratorCoroutine()
        {
            TColorMap colorMap = _colorMapFactory.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                while (_vertexEnumerator.MoveNext())
                {
                    TVertex vertex = _vertexEnumerator.Current;

                    if (!colorMap.TryGetValue(vertex, out Color vertexColor))
                        vertexColor = Color.None;

                    if (vertexColor != Color.None && vertexColor != Color.White)
                        continue;

                    yield return Step.Create(DfsStepKind.StartVertex, vertex, default(TEdge));

                    var steps = new BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                        TGraphConcept>(Graph, vertex, colorMap, GraphConcept);
                    using (IEnumerator<Step<DfsStepKind, TVertex, TEdge>> stepEnumerator = steps.GetEnumerator())
                    {
                        while (stepEnumerator.MoveNext())
                            yield return stepEnumerator.Current;
                    }
                }
            }
            finally
            {
                _colorMapFactory.Release(Graph, colorMap);
            }
        }
    }
}
