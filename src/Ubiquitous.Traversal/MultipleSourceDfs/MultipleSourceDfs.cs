namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public static class MultipleSourceDfs<
        TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static MultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
            Create<TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>(
                TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
                TVertexEnumerablePolicy vertexEnumerablePolicy, TStepPolicy stepPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
            where TVertexEnumerablePolicy : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
            where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return new MultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>(
                graphPolicy, colorMapPolicy, vertexEnumerablePolicy, stepPolicy);
        }
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct MultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator,
        TEdgeEnumerator, TColorMap, TStep,
        TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
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
            Traverse(TGraph graph, TVertexEnumerable vertexCollection, TColorMap colorMap)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>(
                graph, vertexCollection, colorMap, 0,
                GraphPolicy, ColorMapPolicy, VertexEnumerablePolicy, StepPolicy);
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection, TColorMap colorMap, int stackCapacity)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TVertexEnumerablePolicy, TStepPolicy>(
                graph, vertexCollection, colorMap, stackCapacity,
                GraphPolicy, ColorMapPolicy, VertexEnumerablePolicy, StepPolicy);
        }
    }
}
