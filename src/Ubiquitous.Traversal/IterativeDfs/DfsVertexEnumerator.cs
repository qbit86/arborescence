// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    internal struct DfsVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
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

        internal DfsVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _current = default;
            _state = 0;

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;
        }

        internal TGraphPolicy GraphPolicy => _graphPolicy;
        internal TColorMapPolicy ColorMapPolicy => _colorMapPolicy;

        public bool MoveNext() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();

        public DfsVertexStep<TVertex> Current => _current;

        public void Dispose() => throw new NotImplementedException();
    }
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
