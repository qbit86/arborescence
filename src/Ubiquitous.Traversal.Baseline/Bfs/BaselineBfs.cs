namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public static class BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            Create<TGraphPolicy, TColorMapPolicy>(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
            where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        {
            return new BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graphPolicy, colorMapPolicy);
        }
    }

    public readonly struct BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        public BaselineBfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }

        public BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graph, startVertex, 0, GraphPolicy, ColorMapPolicy);
        }

        public BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>
            Traverse(TGraph graph, TVertex startVertex, int queueCapacity)
        {
            return new BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graph, startVertex, queueCapacity, GraphPolicy, ColorMapPolicy);
        }
    }
}
