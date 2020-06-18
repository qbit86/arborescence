namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct GenericSearch<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer, TExploredSet,
        TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TVertexContainerPolicy : IContainerPolicy<TVertexContainer, TVertex>
        where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TVertexContainerPolicy VertexContainerPolicy { get; }
        private TExploredSetPolicy ExploredSetPolicy { get; }

        public GenericSearch(TGraphPolicy graphPolicy,
            TVertexContainerPolicy vertexContainerPolicy, TExploredSetPolicy exploredSetPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (vertexContainerPolicy == null)
                throw new ArgumentNullException(nameof(vertexContainerPolicy));

            if (exploredSetPolicy == null)
                throw new ArgumentNullException(nameof(exploredSetPolicy));

            GraphPolicy = graphPolicy;
            ExploredSetPolicy = exploredSetPolicy;
            VertexContainerPolicy = vertexContainerPolicy;
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
