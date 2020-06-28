namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    public static class GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static GenericSearch<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
                TGraphPolicy, TFringePolicy, TExploredSetPolicy>
            Create<TGraphPolicy, TFringePolicy, TExploredSetPolicy>(
                TGraphPolicy graphPolicy, TFringePolicy vertexContainerPolicy, TExploredSetPolicy exploredSetPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TFringePolicy : IContainerPolicy<TFringe, TVertex>
            where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
        {
            return new GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
                TGraphPolicy, TFringePolicy, TExploredSetPolicy>(
                graphPolicy, vertexContainerPolicy, exploredSetPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
