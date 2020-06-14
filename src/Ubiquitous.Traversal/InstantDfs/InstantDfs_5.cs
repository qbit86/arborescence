namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    public static class InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            Create<TGraphPolicy, TColorMapPolicy>(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        {
            return new InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                graphPolicy, colorMapPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
