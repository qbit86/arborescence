namespace Ubiquitous.Traversal
{
    public readonly partial struct IterativeDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy,
        TColorMapPolicy>
    {
        public DfsSingleComponentEdgeEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> EnumerateEdges(TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            return new DfsSingleComponentEdgeEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy>(GraphPolicy, ColorMapPolicy, graph, startVertex, colorMap);
        }
    }
}
