namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertices : IEnumerable<TVertex>
        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertices Vertices { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal DfsForestStepCollection(TGraph graph, TVertices vertices,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept, TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(vertices != null);
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            Vertices = vertices;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
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

                    var stack = new List<DfsStackFrame<TVertex, TEdge, TEdges>>();
                    var steps = new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdges,
                        TColorMap, List<DfsStackFrame<TVertex, TEdge, TEdges>>,
                        TVertexConcept, TEdgeConcept>(
                        Graph, vertex, colorMap, stack, VertexConcept, EdgeConcept);
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
