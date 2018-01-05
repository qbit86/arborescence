namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct DfsBaseline<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
    {
        // ReSharper disable UnassignedGetOnlyAutoProperty
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new DfsBaselineTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return new DfsBaselineForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, ColorMapFactoryConcept);
        }
    }
}
