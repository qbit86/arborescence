#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
#else
namespace Arborescence.Models.Compatibility
#endif
{
    using System;
    using System.Diagnostics;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    using EdgeEnumerator = System.ArraySegment<Int32Endpoints>.Enumerator;
#else
    using EdgeEnumerator = System.Collections.Generic.IEnumerator<Int32Endpoints>;
#endif

    /// <summary>
    /// Represents a forward-traversable graph.
    /// </summary>
    public sealed class MutableSimpleIncidenceGraph :
        IHeadIncidence<int, Int32Endpoints>,
        ITailIncidence<int, Int32Endpoints>,
        IOutEdgesIncidence<int, EdgeEnumerator>,
        IGraphBuilder<SimpleIncidenceGraph, int, Int32Endpoints>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 4;

        private int _edgeCount;
        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<Int32Endpoints>> _outEdgesByVertex;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableSimpleIncidenceGraph"/> class.
        /// </summary>
        /// <param name="initialVertexCount">The initial number of vertices.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialVertexCount"/> is less than zero.
        /// </exception>
        public MutableSimpleIncidenceGraph(int initialVertexCount = 0)
        {
            if (initialVertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(initialVertexCount));

            _outEdgesByVertex = ArrayPrefixBuilder.EnsureSize(_outEdgesByVertex, initialVertexCount, true);
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
            _edgeCount = 0;
            for (int vertex = 0; vertex < _outEdgesByVertex.Count; ++vertex)
                _outEdgesByVertex[vertex] = ArrayPrefixBuilder.Release(_outEdgesByVertex[vertex], false);
            _outEdgesByVertex = ArrayPrefixBuilder.Release(_outEdgesByVertex, true);
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
        public bool TryAdd(int tail, int head, out Int32Endpoints edge)
        {
            bool result = tail >= 0 && head >= 0;
            edge = result ? UncheckedAdd(tail, head) : default;
            return result;
        }

        /// <inheritdoc/>
        public SimpleIncidenceGraph ToGraph()
        {
            int n = VertexCount;
            int m = EdgeCount;

            int dataLength = 2 + n;
#if NET5_0_OR_GREATER
            int[] data = GC.AllocateUninitializedArray<int>(dataLength);
            Int32Endpoints[] edgesOrderedByTail = GC.AllocateUninitializedArray<Int32Endpoints>(m);
#else
            int[] data = new int[dataLength];
            var edgesOrderedByTail = new Int32Endpoints[m];
#endif
            data[0] = n;
            data[1] = m;

            Span<int> destUpperBoundByVertex = data.AsSpan(2);
            for (int vertex = 0; vertex < n; ++vertex)
            {
                ReadOnlySpan<Int32Endpoints> currentOutEdges = _outEdgesByVertex[vertex].AsSpan();
                int currentLowerBound = vertex > 0 ? destUpperBoundByVertex[vertex - 1] : 0;
                Span<Int32Endpoints> destCurrentOutEdges =
                    edgesOrderedByTail.AsSpan(currentLowerBound, currentOutEdges.Length);
                currentOutEdges.CopyTo(destCurrentOutEdges);
                destUpperBoundByVertex[vertex] = currentLowerBound + currentOutEdges.Length;
            }

            return new(data, edgesOrderedByTail);
        }

        /// <inheritdoc/>
        public bool TryGetHead(Int32Endpoints edge, out int head)
        {
            head = edge.Head;
            return unchecked((uint)head < (uint)VertexCount);
        }

        /// <inheritdoc/>
        public EdgeEnumerator EnumerateOutEdges(int vertex)
        {
            if (unchecked((uint)vertex >= (uint)_outEdgesByVertex.Count))
                return ArraySegmentHelpers.EmptyEnumerator<Int32Endpoints>();

            ArrayPrefix<Int32Endpoints> outEdges = _outEdgesByVertex[vertex];
            if (outEdges.Array is null)
                return ArraySegmentHelpers.EmptyEnumerator<Int32Endpoints>();

            return ArraySegmentHelpers.GetEnumerator<Int32Endpoints>(new(outEdges.Array, 0, outEdges.Count));
        }

        /// <inheritdoc/>
        public bool TryGetTail(Int32Endpoints edge, out int tail)
        {
            tail = edge.Tail;
            return unchecked((uint)tail < (uint)VertexCount);
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
        public Int32Endpoints Add(int tail, int head)
        {
            if (tail < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(tail));

            if (head < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(head));

            return UncheckedAdd(tail, head);
        }

        private Int32Endpoints UncheckedAdd(int tail, int head)
        {
            Debug.Assert(tail >= 0, "tail >= 0");

            int newVertexCountCandidate = Math.Max(tail, head) + 1;
            EnsureVertexCount(newVertexCountCandidate);

            if (_outEdgesByVertex[tail].Array is null)
                _outEdgesByVertex[tail] = ArrayPrefixBuilder.Create<Int32Endpoints>(InitialOutDegree);

            var edge = new Int32Endpoints(tail, head);
            _outEdgesByVertex[tail] = ArrayPrefixBuilder.Add(_outEdgesByVertex[tail], edge, false);
            ++_edgeCount;

            return edge;
        }
    }
}
