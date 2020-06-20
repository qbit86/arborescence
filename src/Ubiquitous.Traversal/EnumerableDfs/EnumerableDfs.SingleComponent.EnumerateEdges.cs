namespace Ubiquitous.Traversal
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct LegacyDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy,
        TColorMapPolicy>
    {
        public DfsSingleComponentEdgeEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> EnumerateEdges(TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            return new DfsSingleComponentEdgeEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(GraphPolicy, ColorMapPolicy, graph, startVertex, colorMap);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
