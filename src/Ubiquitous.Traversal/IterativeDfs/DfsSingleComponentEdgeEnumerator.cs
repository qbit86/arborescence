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
        private int _state;

        internal DfsSingleComponentEdgeEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _state = 1;
        }

        public DfsSingleComponentEdgeEnumerator<
            TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> GetEnumerator()
        {
            DfsSingleComponentEdgeEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy> ator = this;
            ator.Reset();
            return ator;
        }

        IEnumerator<DfsStep<TEdge>> IEnumerable<DfsStep<TEdge>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            if (_state == -1)
                return;

            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (_state <= 0)
                return false;

            throw new NotImplementedException();
        }

        public void Reset()
        {
            ThrowIfNotValid();

            _state = 1;
            throw new NotImplementedException();
        }

        public DfsStep<TEdge> Current
        {
            get
            {
                ThrowIfNotValid();

                Assert(_state > 0, "_state > 0");
                throw new NotImplementedException();
            }
        }

        object IEnumerator.Current => Current;

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
