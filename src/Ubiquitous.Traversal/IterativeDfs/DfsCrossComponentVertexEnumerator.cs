// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsCrossComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private DfsVertexStep<TVertex> _current;
        private int _state;

        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;

        public DfsCrossComponentVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _current = default;
            _state = 0;

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
        }
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
