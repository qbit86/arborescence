namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IHeadPolicy<TGraph, TVertex, TEdge>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TExploredSetPolicy ExploredSetPolicy { get; }

        public EnumerableBfs(TGraphPolicy graphPolicy, TExploredSetPolicy exploredSetPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            GraphPolicy = graphPolicy;
            ExploredSetPolicy = exploredSetPolicy;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
