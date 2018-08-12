namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsForestStepCollection<TGraph, TVertex, TEdge,
            TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerableConcept : IEnumerableConcept<TVertexEnumerable, TVertexEnumerator>
    {
        private TVertexEnumerator _vertexEnumerator;

        private TColorMapConcept _colorMapConcept;

        private TGraph Graph { get; }

        private TGraphConcept GraphConcept { get; }

        private TVertexEnumerableConcept VertexEnumerableConcept { get; }

        internal BaselineDfsForestStepCollection(TGraph graph, TVertexEnumerable vertexCollection,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept,
            TVertexEnumerableConcept vertexEnumerableConcept)
        {
            Assert(vertexCollection != null);
            Assert(colorMapConcept != null);

            Graph = graph;
            GraphConcept = graphConcept;
            _colorMapConcept = colorMapConcept;
            VertexEnumerableConcept = vertexEnumerableConcept;

            // TODO: Delay requesting enumerator.
            _vertexEnumerator = VertexEnumerableConcept.GetEnumerator(vertexCollection);
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
