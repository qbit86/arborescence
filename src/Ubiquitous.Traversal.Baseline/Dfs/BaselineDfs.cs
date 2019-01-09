namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public static class BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>
            Create<TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy, TStepPolicy stepPolicy)
            where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
            where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
        {
            return new BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graphPolicy, colorMapPolicy, stepPolicy);
        }
    }

    public readonly struct BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
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

        public BaselineDfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy, TStepPolicy stepPolicy)
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

        public IEnumerable<TStep>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TStep,
                TGraphPolicy, TColorMapPolicy, TStepPolicy>(
                graph, startVertex, GraphPolicy, ColorMapPolicy, StepPolicy);
        }
    }
}
