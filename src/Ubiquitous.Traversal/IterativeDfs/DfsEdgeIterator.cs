// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    internal struct DfsEdgeIterator<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;

        internal DfsStep<TEdge> _current;
        private TVertex _currentVertex;
        private int _state;

        internal DfsEdgeIterator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;

            _current = new DfsStep<TEdge>(DfsStepKind.None, default);
            _currentVertex = startVertex;
            _state = 1;
        }

        internal bool MoveNext()
        {
            if (_state <= 0)
                return false;

            throw new NotImplementedException();
        }

        internal void Reset(TVertex startVertex)
        {
            Assert(_state > 0, "_state > 0");

            _current = new DfsStep<TEdge>(DfsStepKind.None, default);
            _currentVertex = startVertex;
            _state = 1;
            throw new NotImplementedException();
        }

        internal void Dispose()
        {
            if (_state == -1)
                return;

            _current = default;
            _currentVertex = default;
            _state = -1;
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            _colorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
}
