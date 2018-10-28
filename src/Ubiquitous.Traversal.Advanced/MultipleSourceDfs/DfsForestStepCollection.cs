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
        where TVertexEnumerableConcept : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
    {
        internal DfsForestStepCollection(TGraph graph, TVertexEnumerable vertexCollection, int stackCapacity,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept,
            TVertexEnumerableConcept vertexEnumerableConcept)
        {
            Assert(vertexCollection != null);
            Assert(colorMapConcept != null);

            Graph = graph;
            VertexCollection = vertexCollection;
            StackCapacity = stackCapacity;
            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
            VertexEnumerableConcept = vertexEnumerableConcept;
        }

        private TGraph Graph { get; }
        private TVertexEnumerable VertexCollection { get; }
        public int StackCapacity { get; }
        private TGraphConcept GraphConcept { get; }
        private TColorMapConcept ColorMapConcept { get; }
        private TVertexEnumerableConcept VertexEnumerableConcept { get; }

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
