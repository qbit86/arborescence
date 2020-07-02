namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct GenericSearch<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet, TGraphPolicy, TFringePolicy, TExploredSetPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TFringePolicy : IContainerPolicy<TFringe, TVertex>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TFringePolicy FringePolicy { get; }
        private TExploredSetPolicy ExploredSetPolicy { get; }

        public GenericSearch(TGraphPolicy graphPolicy,
            TFringePolicy fringePolicy, TExploredSetPolicy exploredSetPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (fringePolicy == null)
                throw new ArgumentNullException(nameof(fringePolicy));

            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            GraphPolicy = graphPolicy;
            ExploredSetPolicy = exploredSetPolicy;
            FringePolicy = fringePolicy;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
