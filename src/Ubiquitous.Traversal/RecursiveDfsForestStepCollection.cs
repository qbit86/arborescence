namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal sealed class RecursiveDfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertices : IEnumerable<TVertex>
        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
    {
        private RecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept> Impl { get; }

        private TVertices Vertices { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        internal RecursiveDfsForestStepCollection(RecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
            TVertexConcept, TEdgeConcept> impl, TVertices vertices, TColorMapFactoryConcept colorMapFactoryConcept)
        {
            Assert(vertices != null);
            Assert(colorMapFactoryConcept != null);

            Impl = impl;
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
            TColorMap colorMap = ColorMapFactoryConcept.Acquire(Impl.Graph);
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

                    IEnumerable<Step<DfsStepKind, TVertex, TEdge>> steps = Impl.Traverse(vertex, colorMap);
                    foreach (var step in steps)
                        yield return step;
                }
            }
            finally
            {
                ColorMapFactoryConcept.Release(Impl.Graph, colorMap);
            }
        }
    }
}
