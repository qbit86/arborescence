﻿namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdges,
        TVertexConcept, TEdgeConcept>

        where TEdges : IEnumerable<TEdge>

        where TVertexConcept : struct, IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : struct, IEdgeConcept<TGraph, TVertex, TEdge>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }


        public RecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>
        TraverseRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var impl = new RecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new RecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(impl, startVertex, colorMapFactoryConcept);
        }

        public RecursiveDfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>
        TraverseRecursively<TVertices, TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var impl = new RecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new RecursiveDfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(impl, vertices, colorMapFactoryConcept);
        }

        public NonRecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>
        TraverseNonRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var impl = new NonRecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new NonRecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(impl, startVertex, colorMapFactoryConcept);
        }
    }
}
