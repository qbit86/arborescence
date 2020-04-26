namespace Ubiquitous.Traversal
{
    public readonly partial struct IterativeDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> EnumerateVertices(TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            return new DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(GraphPolicy, ColorMapPolicy, graph, startVertex, colorMap);
        }
    }
}
