namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    public static class GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer, TExploredSet>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static GenericSearch<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer, TExploredSet,
                TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>
            Create<TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>(TGraphPolicy graphPolicy,
                TVertexContainerPolicy vertexContainerPolicy, TExploredSetPolicy exploredSetPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TVertexContainerPolicy : IContainerPolicy<TVertexContainer, TVertex>
            where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
        {
            return new GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer, TExploredSet,
                TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>(
                graphPolicy, vertexContainerPolicy, exploredSetPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
