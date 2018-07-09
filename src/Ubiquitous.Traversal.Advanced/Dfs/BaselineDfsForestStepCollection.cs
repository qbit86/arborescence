namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TGraphConcept, TColorMapConcept>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TVertexEnumerator _vertexEnumerator;

        private TColorMapConcept _colorMapConcept;

        private TGraph Graph { get; }

        private TGraphConcept GraphConcept { get; }

        internal BaselineDfsForestStepCollection(TGraph graph, TVertexEnumerator vertexEnumerator,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            Assert(vertexEnumerator != null);
            Assert(colorMapConcept != null);

            _vertexEnumerator = vertexEnumerator;

            Graph = graph;
            GraphConcept = graphConcept;
            _colorMapConcept = colorMapConcept;
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
            TColorMap colorMap = _colorMapConcept.Acquire();
            if (colorMap == null)
                yield break;

            try
            {
                while (_vertexEnumerator.MoveNext())
                {
                    TVertex vertex = _vertexEnumerator.Current;

                    if (!_colorMapConcept.TryGet(colorMap, vertex, out Color vertexColor))
                        vertexColor = Color.None;

                    if (vertexColor != Color.None && vertexColor != Color.White)
                        continue;

                    yield return Step.Create(DfsStepKind.StartVertex, vertex, default(TEdge));

                    var steps = new BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                        TGraphConcept, TColorMapConcept>(Graph, vertex, colorMap, GraphConcept, _colorMapConcept);
                    using (IEnumerator<Step<DfsStepKind, TVertex, TEdge>> stepEnumerator = steps.GetEnumerator())
                    {
                        while (stepEnumerator.MoveNext())
                            yield return stepEnumerator.Current;
                    }
                }
            }
            finally
            {
                _colorMapConcept.Release(colorMap);
            }
        }
    }
}
