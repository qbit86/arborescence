namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Collections;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct InstantBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }

        public InstantBfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        private void TraverseCore<TContainer, THandler>(
            TGraph graph, TContainer queue, TColorMap colorMap, THandler handler)
            where TContainer : IContainer<TVertex>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(queue != null, "queue != null");
            Debug.Assert(handler != null, "handler != null");

            throw new NotImplementedException();
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
