namespace Ubiquitous.Traversal
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> EnumerateVertices(TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            return new DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(GraphPolicy, ColorMapPolicy, graph, startVertex, colorMap);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
