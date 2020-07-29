namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public sealed class MutableIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArrayPrefixEnumerator<int>>,
        IGraphBuilder<IndexedIncidenceGraph, int, int>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 4;

        private ArrayPrefix<int> _headByEdge;
        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<int>> _outEdgesByVertex;
        private ArrayPrefix<int> _tailByEdge;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableIndexedIncidenceGraph"/> class.
        /// </summary>
        /// <param name="initialVertexCount">The initial number of vertices.</param>
        /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
        /// </exception>
        public MutableIndexedIncidenceGraph(int initialVertexCount, int edgeCapacity = 0)
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
        public int EdgeCount => _headByEdge.Count;

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

            int max = Math.Max(tail, head);
            EnsureVertexCount(max + 1);

            Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");
            int newEdgeIndex = _headByEdge.Count;
            _tailByEdge = ArrayPrefixBuilder.Add(_tailByEdge, tail, false);
            _headByEdge = ArrayPrefixBuilder.Add(_headByEdge, head, false);

            if (_outEdgesByVertex[tail].Array is null)
                _outEdgesByVertex[tail] = ArrayPrefixBuilder.Create<int>(InitialOutDegree);
            _outEdgesByVertex[tail] = ArrayPrefixBuilder.Add(_outEdgesByVertex[tail], newEdgeIndex, false);

            edge = newEdgeIndex;
            return true;
        }

        /// <inheritdoc/>
        public IndexedIncidenceGraph ToGraph()
        {
            int n = VertexCount;
            int m = EdgeCount;

            int dataLength = 1 + n + m + m + m;
#if NET5
            int[] data = GC.AllocateUninitializedArray<int>(dataLength);
#else
            var data = new int[dataLength];
#endif
            data[0] = n;

            Span<int> destUpperBoundByVertex = data.AsSpan(1, n);
            Span<int> destReorderedEdges = data.AsSpan(1 + n, m);
            for (int vertex = 0, currentLowerBound = 0; vertex < n; ++vertex)
            {
                ReadOnlySpan<int> currentOutEdges = _outEdgesByVertex[vertex].AsSpan();
                Span<int> destCurrentOutEdges = destReorderedEdges.Slice(currentLowerBound, currentOutEdges.Length);
                currentOutEdges.CopyTo(destCurrentOutEdges);
                int currentUpperBound = currentLowerBound + currentOutEdges.Length;
                destUpperBoundByVertex[vertex] = currentUpperBound;
                currentLowerBound = currentUpperBound;
            }

            Span<int> destHeadByEdge = data.AsSpan(1 + n + m, m);
            _headByEdge.AsSpan().CopyTo(destHeadByEdge);

            Span<int> destTailByEdge = data.AsSpan(1 + n + m + m, m);
            _tailByEdge.AsSpan().CopyTo(destTailByEdge);

            return new IndexedIncidenceGraph(data);
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            if (unchecked((uint)edge > (uint)_headByEdge.Count))
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
            if (unchecked((uint)edge > (uint)_tailByEdge.Count))
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
            if (unchecked((uint)vertex > (uint)_outEdgesByVertex.Count))
                return ArrayPrefixEnumerator<int>.Empty;

            ArrayPrefix<int> outEdges = _outEdgesByVertex[vertex];
            return new ArrayPrefixEnumerator<int>(outEdges.Array ?? Array.Empty<int>(), outEdges.Count);
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
