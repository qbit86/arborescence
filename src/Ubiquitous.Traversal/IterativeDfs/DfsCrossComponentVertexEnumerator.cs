// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsCrossComponentVertexEnumerator<
        TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy> : IEnumerable<DfsStep<TVertex>>, IEnumerator<DfsStep<TVertex>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private DfsStep<TVertex> _current;
        private int _state;

        private TGraphPolicy _graphPolicy;
        private TColorMapPolicy _colorMapPolicy;

        private readonly TGraph _graph;
        private readonly TVertexEnumerator _vertices;
        private readonly TColorMap _colorMap;

        private readonly bool _hasStartVertex;
        private readonly TVertex _startVertex;

        internal DfsCrossComponentVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap,
            bool hasStartVertex, TVertex startVertex)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _current = default;
            _state = 1;

            _graphPolicy = graphPolicy;
            _colorMapPolicy = colorMapPolicy;

            _graph = graph;
            _vertices = vertices;
            _colorMap = colorMap;

            _hasStartVertex = hasStartVertex;
            _startVertex = startVertex;
        }

        public IEnumerator<DfsStep<TVertex>> GetEnumerator()
        {
            DfsCrossComponentVertexEnumerator<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy> ator = this;
            ator.Reset();
            return ator;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<DfsStep<TVertex>> IEnumerable<DfsStep<TVertex>>.GetEnumerator() => GetEnumerator();

        public bool MoveNext() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public DfsStep<TVertex> Current => _current;

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
