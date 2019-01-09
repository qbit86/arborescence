namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public static class Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>
            Create<TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy, TStepPolicy stepPolicy)
            where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
            where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
        {
            return new Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graphPolicy, colorMapPolicy, stepPolicy);
        }
    }

    public readonly struct Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
        TGraphPolicy, TColorMapPolicy, TStepPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
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
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graph, startVertex, 0, GraphPolicy, ColorMapPolicy, StepPolicy);
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>
            Traverse(TGraph graph, TVertex startVertex, int stackCapacity)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graph, startVertex, stackCapacity, GraphPolicy, ColorMapPolicy, StepPolicy);
        }
    }
}
