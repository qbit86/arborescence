// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public struct DfsCrossComponentVertexEnumerator<
        TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> :
        IEnumerable<DfsStep<TVertex>>, IEnumerator<DfsStep<TVertex>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private readonly TVertex _startVertex;
        private int _state;

        private DfsVertexIterator<
            TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy> _vertexIterator;

        internal DfsCrossComponentVertexEnumerator(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy,
            TGraph graph, TVertexEnumerator vertices, TColorMap colorMap,
            bool hasStartVertex, TVertex startVertex)
        {
            Assert(graphPolicy != null, "graphPolicy != null");
            Assert(colorMapPolicy != null, "colorMapPolicy != null");

            _startVertex = startVertex;
            _vertexIterator = new DfsVertexIterator<
                TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                graphPolicy, colorMapPolicy, graph, startVertex, colorMap);
            _state = 1;
        }

        public DfsCrossComponentVertexEnumerator<
                TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            GetEnumerator()
        {
            DfsCrossComponentVertexEnumerator<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
                TGraphPolicy, TColorMapPolicy> ator = this;
            ator.Reset();
            return ator;
        }

        IEnumerator<DfsStep<TVertex>> IEnumerable<DfsStep<TVertex>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            if (_state <= 0)
                return false;

            throw new NotImplementedException();
        }

        public void Reset() => throw new NotImplementedException();

        public DfsStep<TVertex> Current => throw new NotImplementedException();

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            if (_state == -1)
                return;

            _vertexIterator.Dispose();
            _vertexIterator = default;
            _state = -1;
        }
    }
#pragma warning restore CA1710 // Identifiers should have correct suffix
}

// ReSharper restore FieldCanBeMadeReadOnly.Local
