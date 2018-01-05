namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsBaselineForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactory>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertices : IEnumerable<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertices Vertices { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        internal DfsBaselineForestStepCollection(TGraph graph, TVertices vertices,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactory colorMapFactory)
        {
            Assert(vertices != null);
            Assert(colorMapFactory != null);

            Graph = graph;
            Vertices = vertices;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactory = colorMapFactory;
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
            TColorMap colorMap = ColorMapFactory.Acquire(Graph);
            if (colorMap == null)
                yield break;

            try
            {
                foreach (var vertex in Vertices)
                {
                    Color vertexColor;
                    if (!colorMap.TryGetValue(vertex, out vertexColor))
                        vertexColor = Color.None;

                    if (vertexColor != Color.None && vertexColor != Color.White)
                        continue;

                    yield return Step.Create(DfsStepKind.StartVertex, vertex, default(TEdge));

                    var steps = new DfsBaselineStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
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
                ColorMapFactory.Release(Graph, colorMap);
            }
        }
    }
}
