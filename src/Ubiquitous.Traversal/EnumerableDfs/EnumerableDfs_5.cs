namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    public static class EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static EnumerableDfs<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
            Create<TGraphPolicy, TExploredSetPolicy>(TGraphPolicy graphPolicy, TExploredSetPolicy exploredSetPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TExploredSetPolicy : ISetPolicy<TExploredSet, TVertex>
        {
            return new EnumerableDfs<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>(
                graphPolicy, exploredSetPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
