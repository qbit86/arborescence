namespace Ubiquitous
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsBoostStepCollection<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept>

        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>

        where TEdges : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IIncidenceVertexConcept<TGraph, TVertex, TEdges>
        where TEdgeConcept : IEdgeConcept<TGraph, TVertex, TEdge>
    {
        private TGraph Graph { get; }

        private TVertex StartVertex { get; }

        private TColorMap ColorMap { get; }

        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        internal DfsBoostStepCollection(TGraph graph, TVertex startVertex, TColorMap colorMap,
            TVertexConcept vertexConcept, TEdgeConcept edgeConcept)
        {
            Assert(colorMap != null);
            Assert(vertexConcept != null);
            Assert(edgeConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            ColorMap = colorMap;
            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
        }

        public DfsStepEnumerator<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept>
            GetEnumerator()
        {
            return CreateEnumerator();
        }

        IEnumerator<Step<DfsStepKind, TVertex, TEdge>> IEnumerable<Step<DfsStepKind, TVertex, TEdge>>.GetEnumerator()
        {
            return CreateEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CreateEnumerator();
        }

        private DfsStepEnumerator<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept>
            CreateEnumerator()
        {
            return new DfsStepEnumerator<TGraph, TVertex, TEdge, TEdges, TColorMap, TVertexConcept, TEdgeConcept>(
                Graph, StartVertex, ColorMap, VertexConcept, EdgeConcept);
        }
    }
}
