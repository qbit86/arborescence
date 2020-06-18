namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    public static class GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TContainer, TExploredSet>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static GenericSearch<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TContainer, TExploredSet,
                TGraphPolicy, TContainerPolicy, TExploredSetPolicy>
            Create<TGraphPolicy, TContainerPolicy, TExploredSetPolicy>(
                TGraphPolicy graphPolicy, TContainerPolicy vertexContainerPolicy, TExploredSetPolicy exploredSetPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TContainerPolicy : IContainerPolicy<TContainer, TVertex>
            where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
        {
            return new GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TContainer, TExploredSet,
                TGraphPolicy, TContainerPolicy, TExploredSetPolicy>(
                graphPolicy, vertexContainerPolicy, exploredSetPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
