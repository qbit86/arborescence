namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
    {
        private struct ListStackFactoryInstance: IFactoryConcept<TGraph, List<DfsStackFrame<TVertex, TEdge, TEdges>>>
        {
            public List<DfsStackFrame<TVertex, TEdge, TEdges>> Acquire(TGraph context)
            {
                return new List<DfsStackFrame<TVertex, TEdge, TEdges>>();
            }

            public void Release(TGraph context, List<DfsStackFrame<TVertex, TEdge, TEdges>> value)
            {
                value.Clear();
            }
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            var stackFactory = default(ListStackFactoryInstance);

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                List<DfsStackFrame<TVertex, TEdge, TEdges>>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept, ListStackFactoryInstance>(graph, startVertex,
                VertexConcept, EdgeConcept, ColorMapFactoryConcept, stackFactory);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, ColorMapFactoryConcept);
        }
    }
}
