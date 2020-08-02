namespace Arborescence.Models
{
    using System;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public sealed class MutableUndirectedIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArrayPrefixEnumerator<int>>,
        IGraphBuilder<IndexedIncidenceGraph, int, int>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 8;

        private int _edgeCount;
        private ArrayPrefix<int> _headByEdge;
        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<int>> _outEdgesByVertex;
        private ArrayPrefix<int> _tailByEdge;

        public MutableUndirectedIndexedIncidenceGraph(int initialVertexCount, int edgeCapacity = 0)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            if (edgeCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

            int effectiveEdgeCapacity = Math.Max(edgeCapacity, DefaultInitialOutDegree);
            _tailByEdge = ArrayPrefixBuilder.Create<int>(effectiveEdgeCapacity);
            _headByEdge = ArrayPrefixBuilder.Create<int>(effectiveEdgeCapacity);
            _outEdgesByVertex = ArrayPrefixBuilder.Create<ArrayPrefix<int>>(initialVertexCount);
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _outEdgesByVertex.Count;

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => _edgeCount;

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
            _headByEdge = ArrayPrefixBuilder.Release(_headByEdge, false);
            _tailByEdge = ArrayPrefixBuilder.Release(_tailByEdge, false);
        }

        /// <inheritdoc/>
        public bool TryAdd(int tail, int head, out int edge) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IndexedIncidenceGraph ToGraph() => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            if (unchecked((uint)edge >= (uint)_headByEdge.Count))
            {
                head = default;
                return false;
            }

            head = _headByEdge[edge];
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail)
        {
            if (unchecked((uint)edge >= (uint)_tailByEdge.Count))
            {
                tail = default;
                return false;
            }

            tail = _tailByEdge[edge];
            return true;
        }

        /// <inheritdoc/>
        public ArrayPrefixEnumerator<int> EnumerateOutEdges(int vertex)
        {
            if (unchecked((uint)vertex >= (uint)_outEdgesByVertex.Count))
                return ArrayPrefixEnumerator<int>.Empty;

            ArrayPrefix<int> outEdges = _outEdgesByVertex[vertex];
            return new ArrayPrefixEnumerator<int>(outEdges.Array ?? Array.Empty<int>(), outEdges.Count);
        }
    }
}
