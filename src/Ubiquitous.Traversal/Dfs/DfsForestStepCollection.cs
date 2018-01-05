namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept, TStackFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertices : IEnumerable<TVertex>
        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdges>>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
        where TStackFactoryConcept : IFactoryConcept<TGraph, TStack>
    {
        private TGraph Graph { get; }

        private TVertices Vertices { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        private TStackFactoryConcept StackFactoryConcept { get; }

        internal DfsForestStepCollection(TGraph graph, TVertices vertices,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactoryConcept colorMapFactoryConcept, TStackFactoryConcept stackFactoryConcept)
        {
            Assert(vertices != null);
            Assert(colorMapFactoryConcept != null);
            Assert(stackFactoryConcept != null);

            Graph = graph;
            Vertices = vertices;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactoryConcept = colorMapFactoryConcept;
            StackFactoryConcept = stackFactoryConcept;
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

                    var stack = StackFactoryConcept.Acquire(Graph);
                    if (stack == null)
                        yield break;

                    try
                    {
                        var steps = new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdges,
                            TColorMap, TStack,
                            TVertexConcept, TEdgeConcept>(
                            Graph, vertex, colorMap, stack, VertexConcept, EdgeConcept);
                        while (steps.MoveNext())
                            yield return steps.Current;
                    }
                    finally
                    {
                        StackFactoryConcept.Release(Graph, stack);
                    }
                }
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }
    }
}
