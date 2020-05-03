namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct IterativeDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        public DfsCrossComponentVertexEnumerator<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            return new DfsCrossComponentVertexEnumerator<
                TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                GraphPolicy, ColorMapPolicy, graph, vertices, colorMap, false, default);
        }

        public DfsCrossComponentVertexEnumerator<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap, TVertex startVertex)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            return new DfsCrossComponentVertexEnumerator<
                TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                GraphPolicy, ColorMapPolicy, graph, vertices, colorMap, true, startVertex);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
