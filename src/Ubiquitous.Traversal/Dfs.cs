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


        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var traversal = new RecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                RecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex, traversal, colorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseRecursively<TVertices, TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var traversal = new RecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                RecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices, traversal, colorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseNonRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var traversal = new NonRecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                NonRecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex, traversal, colorMapFactoryConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            TraverseNonRecursively<TVertices, TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            var traversal = new NonRecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(graph, VertexConcept, EdgeConcept);

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdges, TColorMap,
                NonRecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept>,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, vertices, traversal, colorMapFactoryConcept);
        }
    }
}
