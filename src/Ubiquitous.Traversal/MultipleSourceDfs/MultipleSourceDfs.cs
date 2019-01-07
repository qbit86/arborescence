namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public static class MultipleSourceDfs<
        TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static MultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, Step<DfsStepKind, TVertex, TEdge>,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, StepPolicy<DfsStepKind, TVertex, TEdge>>
            Create<TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy>(
                TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
                TVertexEnumerablePolicy vertexEnumerablePolicy)
            where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
            where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
        {
            return new MultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, Step<DfsStepKind, TVertex, TEdge>,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, StepPolicy<DfsStepKind, TVertex, TEdge>>(
                graphPolicy, colorMapPolicy, vertexEnumerablePolicy, default);
        }
    }

    public readonly struct MultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator,
        TEdgeEnumerator, TColorMap, TStep,
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

        public MultipleSourceDfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
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

        public DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>(
                graph, vertexCollection, 0,
                GraphPolicy, ColorMapPolicy, VertexEnumerablePolicy, StepPolicy);
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection, int stackCapacity)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>(
                graph, vertexCollection, stackCapacity,
                GraphPolicy, ColorMapPolicy, VertexEnumerablePolicy, StepPolicy);
        }
    }
}
