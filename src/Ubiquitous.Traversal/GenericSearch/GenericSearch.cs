namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct GenericSearch<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TContainer, TExploredSet,
        TGraphPolicy, TContainerPolicy, TExploredSetPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TContainerPolicy : IContainerPolicy<TContainer, TVertex>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TContainerPolicy ContainerPolicy { get; }
        private TExploredSetPolicy ExploredSetPolicy { get; }

        public GenericSearch(
            TGraphPolicy graphPolicy, TContainerPolicy containerPolicy, TExploredSetPolicy exploredSetPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (containerPolicy == null)
                throw new ArgumentNullException(nameof(containerPolicy));

            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            GraphPolicy = graphPolicy;
            ExploredSetPolicy = exploredSetPolicy;
            ContainerPolicy = containerPolicy;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
