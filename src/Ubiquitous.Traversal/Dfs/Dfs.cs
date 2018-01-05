namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactory>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : struct, IFactoryConcept<TGraph, TColorMap>
    {
        private struct ListStackFactoryInstance: IFactoryConcept<TGraph, List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>>
        {
            public List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> Acquire(TGraph context)
            {
                return new List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>();
            }

            public void Release(TGraph context, List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> value)
            {
                value.Clear();
            }
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            var stackFactory = default(ListStackFactoryInstance);

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>,
                TVertexConcept, TEdgeConcept, TColorMapFactory, ListStackFactoryInstance>(graph, startVertex,
                VertexConcept, EdgeConcept, ColorMapFactory, stackFactory);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            var stackFactory = default(ListStackFactoryInstance);

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdgeEnumerator, TColorMap,
                List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>,
                TVertexConcept, TEdgeConcept, TColorMapFactory, ListStackFactoryInstance>(graph, vertices,
                VertexConcept, EdgeConcept, ColorMapFactory, stackFactory);
        }
    }
}
