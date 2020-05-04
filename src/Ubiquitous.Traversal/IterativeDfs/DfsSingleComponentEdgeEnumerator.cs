namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsSingleComponentEdgeEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy> : IEnumerable<DfsStep<TEdge>>, IEnumerator<DfsStep<TEdge>>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private readonly TVertex _startVertex;
        private int _state;

        private DfsEdgeIterator<
            TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> _edgeIterator;

        internal DfsSingleComponentEdgeEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _startVertex = startVertex;
            _edgeIterator = new DfsEdgeIterator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                graphPolicy, colorMapPolicy, graph, startVertex, colorMap);
            _state = 1;
        }

        public DfsSingleComponentEdgeEnumerator<
            TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> GetEnumerator()
        {
            DfsSingleComponentEdgeEnumerator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> ator = this;
            ator.Reset();
            return ator;
        }

        IEnumerator<DfsStep<TEdge>> IEnumerable<DfsStep<TEdge>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            if (_state <= 0)
                return false;

            _state = 2;
            return _edgeIterator.MoveNext();
        }

        public void Reset()
        {
            ThrowIfNotValid();

            _edgeIterator.Reset(_startVertex);
            _state = 1;
        }

        public DfsStep<TEdge> Current
        {
            get
            {
                ThrowIfNotValid();

                Assert(_state > 0, "_state > 0");
                switch (_state)
                {
                    case 1:
                        return new DfsStep<TEdge>(DfsStepKind.None, default);
                    default:
                        return _edgeIterator._current;
                }
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            if (_state == -1)
                return;

            _edgeIterator.Dispose();
            _edgeIterator = default;
            _state = -1;
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
