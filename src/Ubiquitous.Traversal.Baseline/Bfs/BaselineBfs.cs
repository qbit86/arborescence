namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public static class BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            Create<TGraphPolicy, TColorMapPolicy>(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetHeadPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return new BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graphPolicy, colorMapPolicy);
        }
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetHeadPolicy<TGraph, TVertex, TEdge>,
        IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
#pragma warning restore CA1815 // Override equals and operator equals on value types
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
            Traverse(TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            return new BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graph, startVertex, colorMap, 0, GraphPolicy, ColorMapPolicy);
        }

        public BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>
            Traverse(TGraph graph, TVertex startVertex, TColorMap colorMap, int queueCapacity)
        {
            return new BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(
                graph, startVertex, colorMap, queueCapacity, GraphPolicy, ColorMapPolicy);
        }
    }
}
