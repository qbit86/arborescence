namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public readonly partial struct DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        internal DfsTreeStepCollection(TGraph graph, TVertex startVertex, int stackCapacity,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            Assert(colorMapPolicy != null);

            Graph = graph;
            StartVertex = startVertex;
            StackCapacity = stackCapacity;
            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        private TGraph Graph { get; }
        private TVertex StartVertex { get; }
        private int StackCapacity { get; }
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }

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
