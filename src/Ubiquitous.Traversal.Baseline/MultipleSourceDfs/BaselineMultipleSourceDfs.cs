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
                TEdgeEnumerator, TColorMap, Step<DfsStepKind, TVertex, TEdge>,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, StepPolicy<DfsStepKind, TVertex, TEdge>>
            Create<TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>(
                TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
                TVertexEnumerablePolicy vertexEnumerablePolicy)
            where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
            where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
        {
            return new BaselineMultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator,
                TEdgeEnumerator, TColorMap, Step<DfsStepKind, TVertex, TEdge>,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, StepPolicy<DfsStepKind, TVertex, TEdge>>(
                graphPolicy, colorMapPolicy, vertexEnumerablePolicy, default);
        }
    }

    public readonly struct BaselineMultipleSourceDfs<TGraph, TVertex, TEdge,
        TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
        TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private TGraphPolicy GraphPolicy { get; }

        private TColorMapPolicy ColorMapPolicy { get; }

        private TVertexEnumerablePolicy VertexEnumerablePolicy { get; }

        private TStepPolicy StepPolicy { get; }

        public BaselineMultipleSourceDfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TVertexEnumerablePolicy vertexEnumerablePolicy, TStepPolicy stepPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            if (vertexEnumerablePolicy == null)
                throw new ArgumentNullException(nameof(vertexEnumerablePolicy));

            if (stepPolicy == null)
                throw new ArgumentNullException(nameof(stepPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
            VertexEnumerablePolicy = vertexEnumerablePolicy;
            StepPolicy = stepPolicy;
        }

        public IEnumerable<TStep> Traverse(TGraph graph, TVertexEnumerable vertexCollection)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>(
                graph, vertexCollection,
                GraphPolicy, ColorMapPolicy, VertexEnumerablePolicy, StepPolicy);
        }
    }
}
