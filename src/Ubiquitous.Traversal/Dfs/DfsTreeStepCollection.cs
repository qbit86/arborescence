namespace Ubiquitous
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStack,
        TVertexConcept, TEdgeConcept, TColorMapFactory, TStackFactory>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TStack : IList<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactoryConcept<TGraph, TColorMap>
        where TStackFactory : IFactoryConcept<TGraph, TStack>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        private TStackFactory StackFactory { get; }

        internal DfsTreeStepCollection(TGraph graph, TVertex startVertex,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept,
            TColorMapFactory colorMapFactory, TStackFactory stackFactory)
        {
            Assert(colorMapFactory != null);
            Assert(stackFactory != null);

            Graph = graph;
            StartVertex = startVertex;
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
                yield return Step.Create(DfsStepKind.StartVertex, StartVertex, default(TEdge));

                var stack = StackFactory.Acquire(Graph);
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
                    StackFactory.Release(Graph, stack);
                }
            }
            finally
            {
                ColorMapFactory.Release(Graph, colorMap);
            }
        }
    }
}
