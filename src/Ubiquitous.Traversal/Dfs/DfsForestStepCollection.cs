namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
        TStepEnumeratorProviderConcept, TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertices : IEnumerable<TVertex>
        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TStepEnumeratorProviderConcept : IStepEnumeratorProviderConcept<
            TGraph, TVertex, TColorMap, IEnumerator<Step<DfsStepKind, TVertex, TEdge>>, TVertexConcept, TEdgeConcept>
        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertices Vertices { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TStepEnumeratorProviderConcept StepEnumeratorProviderConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal DfsForestStepCollection(TGraph graph, TVertices vertices,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept, TStepEnumeratorProviderConcept stepEnumeratorProviderConcept,
            TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(vertices != null);
            Assert(stepEnumeratorProviderConcept != null);
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            Vertices = vertices;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            StepEnumeratorProviderConcept = stepEnumeratorProviderConcept;
            ColorMapFactoryConcept = colorMapFactoryConcept;
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
            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Graph);
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

                    var steps = StepEnumeratorProviderConcept.GetEnumerator(Graph, vertex, colorMap, VertexConcept, EdgeConcept);
                    while (steps.MoveNext())
                        yield return steps.Current;
                }
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }
    }
}
