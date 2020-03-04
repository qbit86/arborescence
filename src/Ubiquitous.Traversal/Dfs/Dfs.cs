namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public static class Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>
            Create<TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy, TStepPolicy stepPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
            where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return new Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graphPolicy, colorMapPolicy, stepPolicy);
        }
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
        TGraphPolicy, TColorMapPolicy, TStepPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }
        private TStepPolicy StepPolicy { get; }

        public Dfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TStepPolicy stepPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            if (stepPolicy == null)
                throw new ArgumentNullException(nameof(stepPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
            StepPolicy = stepPolicy;
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>
            Traverse(TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graph, startVertex, 0, GraphPolicy, ColorMapPolicy, StepPolicy);
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>
            Traverse(TGraph graph, TVertex startVertex, TColorMap colorMap, int stackCapacity)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graph, startVertex, stackCapacity, GraphPolicy, ColorMapPolicy, StepPolicy);
        }
    }
}
