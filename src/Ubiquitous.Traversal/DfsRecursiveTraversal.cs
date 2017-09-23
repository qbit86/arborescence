namespace Ubiquitous
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsRecursiveTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept>

        : ITraversal<TVertex, TEdge, TColorMap>

        where TEdges : IEnumerable<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
    {
        private TGraph Graph { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        internal DfsRecursiveTraversal(TGraph graph,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);

            Graph = graph;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>> Traverse(TVertex vertex, TColorMap colorMap)
        {
            Assert(colorMap != null);

            return new DfsRecursiveStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept>(
                Graph, vertex, colorMap, VertexConcept, EdgeConcept);
        }
    }
}
