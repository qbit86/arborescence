namespace Arborescence.Models
{
    using System;

    public sealed class MutableSimpleIncidenceGraph :
        IIncidenceGraph<int, Endpoints, ArrayPrefixEnumerator<Endpoints>>,
        IGraphBuilder<SimpleIncidenceGraph, int, Endpoints>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<Endpoints>> _outEdgesByVertex;

        public MutableSimpleIncidenceGraph(int initialVertexCount)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            _outEdgesByVertex = ArrayPrefixBuilder.Resize(_outEdgesByVertex, initialVertexCount, true);
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _outEdgesByVertex.Count;

        /// <summary>
        /// Gets the initial number of out-edges for each vertex.
        /// </summary>
        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            for (int vertex = 0; vertex < _outEdgesByVertex.Count; ++vertex)
                _outEdgesByVertex[vertex] = ArrayPrefixBuilder.Release(_outEdgesByVertex[vertex], false);
            _outEdgesByVertex = ArrayPrefixBuilder.Release(_outEdgesByVertex, true);
        }

        public bool TryAdd(int tail, int head, out Endpoints edge) => throw new NotImplementedException();

        public SimpleIncidenceGraph ToGraph() => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryGetHead(Endpoints edge, out int head)
        {
            head = edge.Head;
            return unchecked((uint)head < (uint)VertexCount);
        }

        /// <inheritdoc/>
        public bool TryGetTail(Endpoints edge, out int tail)
        {
            tail = edge.Tail;
            return unchecked((uint)tail < (uint)VertexCount);
        }

        /// <inheritdoc/>
        public ArrayPrefixEnumerator<Endpoints> EnumerateOutEdges(int vertex)
        {
            if (unchecked((uint)vertex >= (uint)_outEdgesByVertex.Count))
                return ArrayPrefixEnumerator<Endpoints>.Empty;

            ArrayPrefix<Endpoints> outEdges = _outEdgesByVertex[vertex];
            return new ArrayPrefixEnumerator<Endpoints>(outEdges.Array ?? Array.Empty<Endpoints>(), outEdges.Count);
        }
    }
}
