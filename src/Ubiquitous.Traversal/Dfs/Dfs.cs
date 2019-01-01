namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public static class Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            Create<TGraphPolicy, TColorMapPolicy>(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
            where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        {
            return new Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graphPolicy, colorMapPolicy);
        }
    }

    public readonly struct Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TGraphPolicy GraphPolicy { get; }

        private TColorMapPolicy ColorMapPolicy { get; }

        public Dfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graph, startVertex, 0,
                GraphPolicy, ColorMapPolicy);
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>
            Traverse(TGraph graph, TVertex startVertex, int stackCapacity)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graph, startVertex, stackCapacity,
                GraphPolicy, ColorMapPolicy);
        }
    }
}
