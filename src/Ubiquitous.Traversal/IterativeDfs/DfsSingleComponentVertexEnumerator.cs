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

        private DfsVertexIterator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
            TGraphPolicy, TColorMapPolicy> _vertexIterator;

        internal DfsSingleComponentVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _state = 1;
            _vertexIterator = new DfsVertexIterator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                graphPolicy, colorMapPolicy, graph, startVertex, colorMap);
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
                    return true;
                default:
                    _state = 3;
                    return _vertexIterator.MoveNext();
            }
        }

        public void Reset()
        {
            ThrowIfNotValid();

            _state = 1;
            _vertexIterator = _vertexIterator.Create(_vertexIterator._startVertex);
        }

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public DfsVertexStep<TVertex> Current
        {
            get
            {
                ThrowIfNotValid();

                Assert(_state > 0, "_state > 0");
                switch (_state)
                {
                    case 1:
                        return new DfsVertexStep<TVertex>(DfsStepKind.None, _vertexIterator._startVertex);
                    case 2:
                        return new DfsVertexStep<TVertex>(DfsStepKind.StartVertex, _vertexIterator._startVertex);
                    default:
                        return _vertexIterator._current;
                }
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
