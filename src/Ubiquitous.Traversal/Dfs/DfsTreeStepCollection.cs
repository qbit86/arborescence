namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept, TStackFactoryConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : IFactoryConcept<TGraph, TColorMap>
        where TStackFactoryConcept : IFactoryConcept<TGraph, TStack>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }

        private TStackFactoryConcept StackFactoryConcept { get; }

        internal DfsTreeStepCollection(TGraph graph, TVertex startVertex,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactoryConcept colorMapFactoryConcept, TStackFactoryConcept stackFactoryConcept)
        {
            Assert(colorMapFactoryConcept != null);
            Assert(stackFactoryConcept != null);

            Graph = graph;
            StartVertex = startVertex;
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
                yield return Step.Create(DfsStepKind.StartVertex, StartVertex, default(TEdge));

                var stack = StackFactoryConcept.Acquire(Graph);
                if (stack == null)
                    yield break;

                try
                {
                    var steps = new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator,
                        TColorMap, TStack,
                        TVertexConcept, TEdgeConcept>(
                        Graph, StartVertex, colorMap, stack, VertexConcept, EdgeConcept);
                    while (steps.MoveNext())
                        yield return steps.Current;
                }
                finally
                {
                    StackFactoryConcept.Release(Graph, stack);
                }
            }
            finally
            {
                ColorMapFactoryConcept.Release(Graph, colorMap);
            }
        }
    }
}
