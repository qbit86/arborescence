#if NETSTANDARD2_1 || NETCOREAPP2_1
namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public sealed class MutableIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArraySegment<int>.Enumerator>,
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
        public MutableIndexedIncidenceGraph(int initialVertexCount = 0, int edgeCapacity = 0)
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

        /// <summary>
        /// Attempts to add the edge with the specified endpoints to the graph.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        /// <param name="edge">
        /// When this method returns, contains the added edge, if the edge was added to the graph successfully;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>
        /// A value indicating whether the edge was added successfully.
        /// <c>true</c> if both <paramref name="tail"/> and <paramref name="head"/> are non-negative;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool TryAdd(int tail, int head, out int edge)
        {
            bool result = tail >= 0 && head >= 0;
            edge = result ? UncheckedAdd(tail, head) : default;
            return result;
        }

        /// <inheritdoc/>
        public IndexedIncidenceGraph ToGraph()
        {
            int n = VertexCount;
            int m = EdgeCount;

            int dataLength = 2 + n + m + m + m;
#if NET5
            int[] data = GC.AllocateUninitializedArray<int>(dataLength);
#else
            int[] data = new int[dataLength];
#endif
            data[0] = n;
            data[1] = m;

            Span<int> destUpperBoundByVertex = data.AsSpan(2, n);
            Span<int> destEdgesOrderedByTail = data.AsSpan(2 + n, m);
            for (int vertex = 0; vertex < n; ++vertex)
            {
                ReadOnlySpan<int> currentOutEdges = _outEdgesByVertex[vertex].AsSpan();
                int currentLowerBound = vertex > 0 ? destUpperBoundByVertex[vertex - 1] : 0;
                Span<int> destCurrentOutEdges = destEdgesOrderedByTail.Slice(currentLowerBound, currentOutEdges.Length);
                currentOutEdges.CopyTo(destCurrentOutEdges);
                destUpperBoundByVertex[vertex] = currentLowerBound + currentOutEdges.Length;
            }

            Span<int> destHeadByEdge = data.AsSpan(2 + n + m, m);
            _headByEdge.AsSpan().CopyTo(destHeadByEdge);

            Span<int> destTailByEdge = data.AsSpan(2 + n + m + m, m);
            _tailByEdge.AsSpan().CopyTo(destTailByEdge);

            return new IndexedIncidenceGraph(data);
        }

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
                _outEdgesByVertex = ArrayPrefixBuilder.EnsureSize(_outEdgesByVertex, vertexCount, true);
        }

        /// <summary>
        /// Adds the edge with the specified endpoints to the graph.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        /// <returns>The added edge.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="tail"/> is less than zero, or <paramref name="head"/> is less than zero.
        /// </exception>
        public int Add(int tail, int head)
        {
            if (tail < 0)
                throw new ArgumentOutOfRangeException(nameof(tail));

            if (head < 0)
                throw new ArgumentOutOfRangeException(nameof(head));

            return UncheckedAdd(tail, head);
        }

        private int UncheckedAdd(int tail, int head)
        {
            Debug.Assert(tail >= 0, "tail >= 0");

            int newVertexCountCandidate = Math.Max(tail, head) + 1;
            EnsureVertexCount(newVertexCountCandidate);

            Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");
            int newEdgeIndex = _headByEdge.Count;
            _tailByEdge = ArrayPrefixBuilder.Add(_tailByEdge, tail, false);
            _headByEdge = ArrayPrefixBuilder.Add(_headByEdge, head, false);

            if (_outEdgesByVertex[tail].Array is null)
                _outEdgesByVertex[tail] = ArrayPrefixBuilder.Create<int>(InitialOutDegree);
            _outEdgesByVertex[tail] = ArrayPrefixBuilder.Add(_outEdgesByVertex[tail], newEdgeIndex, false);

            return newEdgeIndex;
        }
    }
}
#endif
