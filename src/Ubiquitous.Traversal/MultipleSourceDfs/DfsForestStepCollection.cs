namespace Ubiquitous.Traversal
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct DfsForestStepCollection<TGraph, TVertex, TEdge,
            TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TStep, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
        : IEnumerable<TStep>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        internal DfsForestStepCollection(
            TGraph graph, TVertexEnumerable vertexCollection, TColorMap colorMap, int stackCapacity,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TVertexEnumerablePolicy vertexEnumerablePolicy, TStepPolicy stepPolicy)
        {
            Assert(vertexCollection != null);
            Assert(colorMapPolicy != null);

            Graph = graph;
            VertexCollection = vertexCollection;
            ColorMap = colorMap;
            StackCapacity = stackCapacity;
            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
            VertexEnumerablePolicy = vertexEnumerablePolicy;
            StepPolicy = stepPolicy;
        }

        private TGraph Graph { get; }
        private TVertexEnumerable VertexCollection { get; }
        private TColorMap ColorMap { get; }
        private int StackCapacity { get; }
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }
        private TVertexEnumerablePolicy VertexEnumerablePolicy { get; }
        private TStepPolicy StepPolicy { get; }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<TStep> IEnumerable<TStep>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
