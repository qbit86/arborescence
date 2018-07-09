namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TGraphConcept, TColorMapConcept>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TGraph Graph { get; }

        private TVertexEnumerator VertexEnumerator { get; }

        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        internal DfsForestStepCollection(TGraph graph, TVertexEnumerator vertexEnumerator,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            Assert(vertexEnumerator != null);
            Assert(colorMapConcept != null);

            Graph = graph;
            VertexEnumerator = vertexEnumerator;
            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<Step<DfsStepKind, TVertex, TEdge>> IEnumerable<Step<DfsStepKind, TVertex, TEdge>>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
