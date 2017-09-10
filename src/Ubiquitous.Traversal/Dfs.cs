namespace Ubiquitous
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
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startVertex == null)
                throw new ArgumentNullException(nameof(startVertex));

            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var impl = new RecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new RecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(impl, startVertex, colorMapFactoryConcept);
        }

        public NonRecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>
        TraverseNonRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startVertex == null)
                throw new ArgumentNullException(nameof(startVertex));

            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var impl = new NonRecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new NonRecursiveDfsStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(impl, startVertex, colorMapFactoryConcept);
        }
    }
}
