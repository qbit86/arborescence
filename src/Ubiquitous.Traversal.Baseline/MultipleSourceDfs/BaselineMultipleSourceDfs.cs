namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public static class BaselineMultipleSourceDfs<
        TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static BaselineMultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator,
                TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>
            Create<TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>(
                TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
                TVertexEnumerablePolicy vertexEnumerablePolicy)
            where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
            where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
        {
            return new BaselineMultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator,
                TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>(
                graphPolicy, colorMapPolicy, vertexEnumerablePolicy);
        }
    }

    public struct BaselineMultipleSourceDfs<TGraph, TVertex, TEdge,
        TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
    {
        private TGraphPolicy GraphPolicy { get; }

        private TColorMapPolicy ColorMapPolicy { get; }

        private TVertexEnumerablePolicy VertexEnumerablePolicy { get; }

        public BaselineMultipleSourceDfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TVertexEnumerablePolicy vertexEnumerablePolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            if (vertexEnumerablePolicy == null)
                throw new ArgumentNullException(nameof(vertexEnumerablePolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
            VertexEnumerablePolicy = vertexEnumerablePolicy;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>(graph, vertexCollection,
                GraphPolicy, ColorMapPolicy, VertexEnumerablePolicy);
        }
    }
}
