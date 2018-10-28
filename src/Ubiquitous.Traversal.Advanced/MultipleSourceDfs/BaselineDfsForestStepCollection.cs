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
        where TGraphConcept : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerableConcept : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
    {
        private TColorMapConcept _colorMapConcept;

        private TGraph Graph { get; }

        private TVertexEnumerable VertexCollection { get; }

        private TGraphConcept GraphConcept { get; }

        private TVertexEnumerableConcept VertexEnumerableConcept { get; }

        internal BaselineDfsForestStepCollection(TGraph graph, TVertexEnumerable vertexCollection,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept,
            TVertexEnumerableConcept vertexEnumerableConcept)
        {
            Assert(vertexCollection != null);
            Assert(colorMapConcept != null);

            Graph = graph;
            VertexCollection = vertexCollection;
            GraphConcept = graphConcept;
            _colorMapConcept = colorMapConcept;
            VertexEnumerableConcept = vertexEnumerableConcept;
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

            TVertexEnumerator vertexEnumerator = VertexEnumerableConcept.GetEnumerator(VertexCollection);
            try
            {
                if (vertexEnumerator == null)
                    yield break;

                while (vertexEnumerator.MoveNext())
                {
                    TVertex vertex = vertexEnumerator.Current;

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
                vertexEnumerator?.Dispose();
                _colorMapConcept.Release(colorMap);
            }
        }
    }
}
