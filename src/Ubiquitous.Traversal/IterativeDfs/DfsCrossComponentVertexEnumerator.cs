namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    public struct DfsCrossComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;

        public DfsCrossComponentVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
        }
    }
}
