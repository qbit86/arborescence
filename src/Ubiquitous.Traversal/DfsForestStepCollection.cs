namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal sealed class DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap, TTraversal,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertices : IEnumerable<TVertex>
        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TTraversal : ITraversal<TVertex, TEdge, TColorMap>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertices Vertices { get; }

        private TTraversal Traversal { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal DfsForestStepCollection(TGraph graph, TVertices vertices, TTraversal traversal,
            TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(vertices != null);
            Assert(traversal != null);
            Assert(colorMapFactoryConcept != null);

            Graph = graph;
            Traversal = traversal;
            Vertices = vertices;
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

                    IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = Traversal.Traverse(vertex, colorMap);
                    foreach (var step in steps)
                        yield return step;
                }
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }
    }
}
