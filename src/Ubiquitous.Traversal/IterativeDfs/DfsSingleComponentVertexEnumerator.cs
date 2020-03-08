// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsSingleComponentVertexEnumerator<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
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

        private readonly TGraph _graph;
        private readonly TVertex _startVertex;
        private readonly TColorMap _colorMap;

        internal DfsSingleComponentVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertex startVertex, TColorMap colorMap)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _current = default;
            _state = 1;

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;

            _graph = graph;
            _startVertex = startVertex;
            _colorMap = colorMap;
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
