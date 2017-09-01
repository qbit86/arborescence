namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    internal struct DfsImpl<TGraph, TVertex, TEdge, TVertexData, TEdgeData, TEdges,
        TGraphConcept, TVertexConcept, TEdgeConcept>
        where TEdges: IEnumerable<TEdge>
        where TGraphConcept : IGraphConcept<TGraph, TVertex, TEdge, TVertexData, TEdgeData>
        where TVertexConcept : IIncidenceVertexConcept<TEdge, TVertexData, TEdges>
        where TEdgeConcept : IEdgeConcept<TVertex, TEdgeData>
    {
        private TGraphConcept GraphConcept { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        internal DfsImpl(TGraphConcept graphConcept, TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            // Assert: `graphConcept` != null.
            // Assert: `vertexConcept` != null.
            // Assert: `edgeConcept` != null.

            GraphConcept = graphConcept;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        internal DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TColorMap, TColorMapFactoryConcept>
            TraverseRecursively<TColorMap, TColorMapFactoryConcept>(
            TGraph graph, TVertex startVertex, TColorMapFactoryConcept colorMapFactoryConcept)
            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : IMapFactoryConcept<TGraph, TVertex, Color, TColorMap>
        {
            // Assert: `graph` != null.
            // Assert: `startVertex` != null.
            // Assert: `colorMapFactoryConcept` != null.

            return new DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TColorMap, TColorMapFactoryConcept>(graph, startVertex);
        }
    }
}
