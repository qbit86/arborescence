namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TGraphConcept, TColorMapFactory>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TGraph Graph { get; }

        private TVertexEnumerator VertexEnumerator { get; }

        private TGraphConcept GraphConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        internal DfsForestStepCollection(TGraph graph, TVertexEnumerator vertexEnumerator,
            TGraphConcept graphConcept, TColorMapFactory colorMapFactory)
        {
            Assert(vertexEnumerator != null);
            Assert(colorMapFactory != null);

            Graph = graph;
            VertexEnumerator = vertexEnumerator;
            GraphConcept = graphConcept;
            ColorMapFactory = colorMapFactory;
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
