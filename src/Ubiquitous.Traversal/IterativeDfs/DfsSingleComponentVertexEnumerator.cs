// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

    // States:
    // • -1: disposed
    // • 0: not initialized
    // • 1: initialized, but not moved
    // • 2: StartVertex
    // • >2: other step kinds

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy> : IEnumerable<DfsVertexStep<TVertex>>, IEnumerator<DfsVertexStep<TVertex>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private int _state;

        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;

        private readonly TGraph _graph;
        private readonly TVertex _startVertex;
        private readonly TColorMap _colorMap;

        private DfsVertexIterator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> _vertexIterator;

        internal DfsSingleComponentVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _state = 1;

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;

            _graph = graph;
            _startVertex = startVertex;
            _colorMap = colorMap;

            _vertexIterator = default;
        }

        public IEnumerator<DfsVertexStep<TVertex>> GetEnumerator()
        {
            DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy> ator = this;
            ator.Reset();
            return ator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<DfsVertexStep<TVertex>> IEnumerable<DfsVertexStep<TVertex>>.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            ThrowIfNotValid();

            Assert(_state > 0, "_state > 0");
            switch (_state)
            {
                case 1:
                    _state = 2;
                    _vertexIterator = new DfsVertexIterator<
                        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                        _graphPolicy, _colorMapPolicy);
                    return true;
                default:
                    _state = 3;
                    return _vertexIterator.MoveNext();
            }
        }

        public void Reset()
        {
            _state = 1;
            _vertexIterator = default;
        }

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public DfsVertexStep<TVertex> Current
        {
            get
            {
                ThrowIfNotValid();

                if (_state == 1)
                    return new DfsVertexStep<TVertex>(DfsStepKind.None, _startVertex);

                if (_state == 2)
                    return new DfsVertexStep<TVertex>(DfsStepKind.StartVertex, _startVertex);

                return _vertexIterator._current;
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _state = -1;
            _vertexIterator = default;
        }

        private void ThrowIfNotValid()
        {
            if (_state == 0)
                throw new InvalidOperationException();

            if (_state == -1)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
