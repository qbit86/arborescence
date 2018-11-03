namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TGraphPolicy GraphPolicy { get; }

        private TColorMapPolicy ColorMapPolicy { get; }

        public BaselineDfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(graph, startVertex, GraphPolicy, ColorMapPolicy);
        }
    }
}
