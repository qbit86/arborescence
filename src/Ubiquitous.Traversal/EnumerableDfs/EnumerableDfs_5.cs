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

    public static class LegacyDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static LegacyDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            Create<TGraphPolicy, TColorMapPolicy>(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        {
            return new LegacyDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                graphPolicy, colorMapPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
