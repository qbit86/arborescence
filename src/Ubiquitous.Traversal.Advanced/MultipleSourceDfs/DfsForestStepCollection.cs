namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsForestStepCollection<TGraph, TVertex, TEdge,
            TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerableConcept : IEnumerableConcept<TVertexEnumerable, TVertexEnumerator>
    {
        // TODO: Replace passing enumerator because it's not reenumeratable.
        internal DfsForestStepCollection(TGraph graph, TVertexEnumerator vertexEnumerator, int stackCapacity,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept,
            TVertexEnumerableConcept vertexCollectionConcept)
        {
            Assert(vertexEnumerator != null);
            Assert(colorMapConcept != null);

            Graph = graph;
            VertexEnumerator = vertexEnumerator;
            StackCapacity = stackCapacity;
            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
            VertexCollectionConcept = vertexCollectionConcept;
        }

        private TGraph Graph { get; }
        private TVertexEnumerator VertexEnumerator { get; }
        public int StackCapacity { get; }
        private TGraphConcept GraphConcept { get; }
        private TColorMapConcept ColorMapConcept { get; }
        private TVertexEnumerableConcept VertexCollectionConcept { get; }

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
