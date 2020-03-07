namespace Ubiquitous.Traversal
{
    using System;

    public readonly partial struct IterativeDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> EnumerateVertices(TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            throw new NotImplementedException();
        }
    }
}
