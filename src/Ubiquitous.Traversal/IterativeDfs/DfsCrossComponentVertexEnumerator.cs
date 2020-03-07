// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsCrossComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy> : IEnumerable<DfsVertexStep<TVertex>>, IEnumerator<DfsVertexStep<TVertex>>
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

        public IEnumerator<DfsVertexStep<TVertex>> GetEnumerator()
        {
            DfsCrossComponentVertexEnumerator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> ator = this;
            ator.Reset();
            return ator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<DfsVertexStep<TVertex>> IEnumerable<DfsVertexStep<TVertex>>.GetEnumerator() => GetEnumerator();

        public bool MoveNext() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public DfsVertexStep<TVertex> Current => _current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _current = default;
            _state = -1;
        }
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
