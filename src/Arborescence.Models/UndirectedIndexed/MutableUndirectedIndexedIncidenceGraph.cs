#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1

namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public sealed class MutableUndirectedIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArraySegment<int>.Enumerator>,
        IGraphBuilder<UndirectedIndexedIncidenceGraph, int, int>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 8;

        private int _edgeCount;
        private ArrayPrefix<int> _headByEdge;
        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<int>> _outEdgesByVertex;
        private ArrayPrefix<int> _tailByEdge;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableUndirectedIndexedIncidenceGraph"/> class.
        /// </summary>
        /// <param name="initialVertexCount">The initial number of vertices.</param>
        /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
        /// </exception>
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
        public bool TryAdd(int tail, int head, out int edge)
        {
            if (tail < 0 || head < 0)
            {
                edge = default;
                return false;
            }

            int newVertexCountCandidate = Math.Max(tail, head) + 1;
            EnsureVertexCount(newVertexCountCandidate);

            Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");
            int newEdgeIndex = _headByEdge.Count;
            _tailByEdge = ArrayPrefixBuilder.Add(_tailByEdge, tail, false);
            _headByEdge = ArrayPrefixBuilder.Add(_headByEdge, head, false);

            if (_outEdgesByVertex[tail].Array is null)
                _outEdgesByVertex[tail] = ArrayPrefixBuilder.Create<int>(InitialOutDegree);
            _outEdgesByVertex[tail] = ArrayPrefixBuilder.Add(_outEdgesByVertex[tail], newEdgeIndex, false);
            ++_edgeCount;

            if (tail != head)
            {
                int invertedEdge = ~newEdgeIndex;
                if (_outEdgesByVertex[head].Array is null)
                    _outEdgesByVertex[head] = ArrayPrefixBuilder.Create<int>(InitialOutDegree);
                _outEdgesByVertex[head] = ArrayPrefixBuilder.Add(_outEdgesByVertex[head], invertedEdge, false);
            }

            edge = newEdgeIndex;
            return true;
        }

        /// <inheritdoc/>
        public UndirectedIndexedIncidenceGraph ToGraph()
        {
            int n = VertexCount;
            int m = EdgeCount;

            int dataLength = 2 + n + m + m + m + m;
#if NET5
            int[] data = GC.AllocateUninitializedArray<int>(dataLength);
#else
            var data = new int[dataLength];
#endif
            data[0] = n;
            data[1] = m;

            Span<int> destUpperBoundByVertex = data.AsSpan(2, n);
            Span<int> destEdgesOrderedByTail = data.AsSpan(2 + n, m + m);
            for (int vertex = 0; vertex < n; ++vertex)
            {
                ReadOnlySpan<int> currentOutEdges = _outEdgesByVertex[vertex].AsSpan();
                int currentLowerBound = vertex > 0 ? destUpperBoundByVertex[vertex - 1] : 0;
                Span<int> destCurrentOutEdges = destEdgesOrderedByTail.Slice(currentLowerBound, currentOutEdges.Length);
                currentOutEdges.CopyTo(destCurrentOutEdges);
                destUpperBoundByVertex[vertex] = currentLowerBound + currentOutEdges.Length;
            }

            Span<int> destHeadByEdge = data.AsSpan(2 + n + m + m, m);
            _headByEdge.AsSpan().CopyTo(destHeadByEdge);

            Span<int> destTailByEdge = data.AsSpan(2 + n + m + m + m, m);
            _tailByEdge.AsSpan().CopyTo(destTailByEdge);

            return new UndirectedIndexedIncidenceGraph(data);
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            int edgeIndex = edge < 0 ? ~edge : edge;
            ArrayPrefix<int> endpointByEdge = edge < 0 ? _tailByEdge : _headByEdge;
            if (edgeIndex >= endpointByEdge.Count)
            {
                head = default;
                return false;
            }

            head = endpointByEdge[edgeIndex];
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail)
        {
            int edgeIndex = edge < 0 ? ~edge : edge;
            ArrayPrefix<int> endpointByEdge = edge < 0 ? _headByEdge : _tailByEdge;
            if (edgeIndex >= endpointByEdge.Count)
            {
                tail = default;
                return false;
            }

            tail = endpointByEdge[edgeIndex];
            return true;
        }

        /// <inheritdoc/>
        public ArraySegment<int>.Enumerator EnumerateOutEdges(int vertex)
        {
            if (unchecked((uint)vertex >= (uint)_outEdgesByVertex.Count))
                return ArraySegment<int>.Empty.GetEnumerator();

            ArrayPrefix<int> outEdges = _outEdgesByVertex[vertex];
            if (outEdges.Array is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            return new ArraySegment<int>(outEdges.Array, 0, outEdges.Count).GetEnumerator();
        }

        /// <summary>
        /// Ensures that the graph can hold the specified number of vertices without growing.
        /// </summary>
        /// <param name="vertexCount">The number of vertices.</param>
        public void EnsureVertexCount(int vertexCount)
        {
            if (vertexCount > VertexCount)
                _outEdgesByVertex = ArrayPrefixBuilder.Resize(_outEdgesByVertex, vertexCount, true);
        }
    }
}

#endif
