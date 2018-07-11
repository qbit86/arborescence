namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphConcept, TColorMapConcept>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        internal DfsTreeStepCollection(TGraph graph, TVertex startVertex, int stackCapacity,
            TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            Assert(colorMapConcept != null);

            Graph = graph;
            StartVertex = startVertex;
            StackCapacity = stackCapacity;
            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        private TGraph Graph { get; }
        private TVertex StartVertex { get; }
        private int StackCapacity { get; }
        private TGraphConcept GraphConcept { get; }
        private TColorMapConcept ColorMapConcept { get; }

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
