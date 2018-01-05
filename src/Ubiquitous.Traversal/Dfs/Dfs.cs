namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : struct, IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
    {
        // ReSharper disable UnassignedGetOnlyAutoProperty
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactoryConcept ColorMapFactoryConcept { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBaseline(TGraph graph, TVertex startVertex)
        {
            return new DfsBaselineTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBaseline<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return new DfsBaselineForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices,
                VertexConcept, EdgeConcept, ColorMapFactoryConcept);
        }


        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBoost(TGraph graph, TVertex startVertex)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, ColorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseBoost<TVertices>(TGraph graph, TVertices vertices)

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
