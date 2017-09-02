namespace Ubiquitous
{
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdges,
        TVertexConcept, TEdgeConcept>

        where TEdges : IEnumerable<TEdge>

        where TVertexConcept : struct, IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : struct, IEdgeConcept<TGraph, TVertex, TEdge>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }


        public DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TEdges, TColorMap,
            TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>
        TraverseRecursively<TColorMap, TColorMapFactoryConcept>(TGraph graph, TVertex startVertex)

            where TColorMap : IDictionary<TVertex, Color>
            where TColorMapFactoryConcept : struct, IFactoryConcept<TGraph, TColorMap>
        {
            TColorMapFactoryConcept colorMapFactoryConcept = default(TColorMapFactoryConcept);

            return new DfsRecursiveEnumerable<TGraph, TVertex, TEdge, TEdges, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactoryConcept>(graph, startVertex,
                VertexConcept, EdgeConcept, colorMapFactoryConcept);
        }
    }
}
