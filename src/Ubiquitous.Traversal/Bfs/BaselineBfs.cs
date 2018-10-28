namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

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
