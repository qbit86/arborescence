namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TVertices : IEnumerable<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
        where TStackFactory : IFactory<TGraph, TStack>
    {
        private TGraph Graph { get; }

        private TVertices Vertices { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        private TStackFactory StackFactory { get; }

        internal DfsForestStepCollection(TGraph graph, TVertices vertices,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactory colorMapFactory, TStackFactory stackFactory)
        {
            Assert(vertices != null);
            Assert(colorMapFactory != null);
            Assert(stackFactory != null);

            Graph = graph;
            Vertices = vertices;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactory = colorMapFactory;
            StackFactory = stackFactory;
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

                    var stack = StackFactory.Acquire(Graph);
                    if (stack == null)
                        yield break;

                    try
                    {
                        var steps = new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator,
                            TColorMap, TStack,
                            TVertexConcept, TEdgeConcept>(
                            Graph, vertex, colorMap, stack, VertexConcept, EdgeConcept);
                        while (steps.MoveNext())
                            yield return steps.Current;
                    }
                    finally
                    {
                        StackFactory.Release(Graph, stack);
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
