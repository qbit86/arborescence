namespace Ubiquitous.Traversal.Advanced
{
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public partial struct DfsForestStepCollection<TGraph, TVertex, TEdge,
            TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
            TColorMap, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>
        : IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
    {
        internal DfsForestStepCollection(TGraph graph, TVertexEnumerable vertexCollection, int stackCapacity,
            TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TVertexEnumerablePolicy vertexEnumerablePolicy)
        {
            Assert(vertexCollection != null);
            Assert(colorMapPolicy != null);

            Graph = graph;
            VertexCollection = vertexCollection;
            StackCapacity = stackCapacity;
            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
            VertexEnumerablePolicy = vertexEnumerablePolicy;
        }

        private TGraph Graph { get; }
        private TVertexEnumerable VertexCollection { get; }
        public int StackCapacity { get; }
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }
        private TVertexEnumerablePolicy VertexEnumerablePolicy { get; }

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
