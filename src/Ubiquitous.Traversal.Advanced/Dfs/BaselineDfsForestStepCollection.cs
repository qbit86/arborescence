namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactory>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private TVertexEnumerator _vertexEnumerator;
        private TColorMapFactory _colorMapFactory;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        private TGraph Graph { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        internal BaselineDfsForestStepCollection(TGraph graph, TVertexEnumerator vertexEnumerator,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactory colorMapFactory)
        {
            Assert(vertexEnumerator != null);
            Assert(colorMapFactory != null);

            _vertexEnumerator = vertexEnumerator;

            Graph = graph;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
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
                    var vertex = _vertexEnumerator.Current;

                    Color vertexColor;
                    if (!colorMap.TryGetValue(vertex, out vertexColor))
                        vertexColor = Color.None;

                    if (vertexColor != Color.None && vertexColor != Color.White)
                        continue;

                    yield return Step.Create(DfsStepKind.StartVertex, vertex, default(TEdge));

                    var steps = new BaselineDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                        TVertexConcept, TEdgeConcept>(Graph, vertex, colorMap, VertexConcept, EdgeConcept);
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
