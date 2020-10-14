namespace Arborescence.Models.Compatibility
{
    using System;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1
    [Obsolete("Please use Arborescence.Models.MutableSimpleIncidenceGraph instead.")]
#endif
    public sealed class MutableSimpleIncidenceGraph :
        IIncidenceGraph<int, Endpoints, ArrayPrefixEnumerator<Endpoints>>,
        IGraphBuilder<SimpleIncidenceGraph, int, Endpoints>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 4;

        private int _edgeCount;
        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<Endpoints>> _outEdgesByVertex;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableSimpleIncidenceGraph"/> class.
        /// </summary>
        /// <param name="initialVertexCount">The initial number of vertices.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="initialVertexCount"/> is less than zero.
        /// </exception>
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

        /// <inheritdoc/>
        /// <returns>A value indicating whether the edge was added successfully.
        /// <c>true</c> if both <paramref name="tail"/> and <paramref name="head"/> are non-negative;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool TryAdd(int tail, int head, out Endpoints edge)
        {
            edge = new Endpoints(tail, head);
            if (tail < 0 || head < 0)
                return false;

            int newVertexCountCandidate = Math.Max(tail, head) + 1;
            EnsureVertexCount(newVertexCountCandidate);

            if (_outEdgesByVertex[tail].Array is null)
                _outEdgesByVertex[tail] = ArrayPrefixBuilder.Create<Endpoints>(InitialOutDegree);
            _outEdgesByVertex[tail] = ArrayPrefixBuilder.Add(_outEdgesByVertex[tail], edge, false);
            ++_edgeCount;

            return true;
        }

        /// <inheritdoc/>
        public SimpleIncidenceGraph ToGraph()
        {
            int n = VertexCount;
            int m = EdgeCount;

            int dataLength = 2 + n;
#if NET5
            int[] data = GC.AllocateUninitializedArray<int>(dataLength);
            Endpoints[] edgesOrderedByTail = GC.AllocateUninitializedArray<Endpoints>(m);
#else
            var data = new int[dataLength];
            var edgesOrderedByTail = new Endpoints[m];
#endif
            data[0] = n;
            data[1] = m;

            Span<int> destUpperBoundByVertex = data.AsSpan(2);
            for (int vertex = 0; vertex < n; ++vertex)
            {
                ReadOnlySpan<Endpoints> currentOutEdges = _outEdgesByVertex[vertex].AsSpan();
                int currentLowerBound = vertex > 0 ? destUpperBoundByVertex[vertex - 1] : 0;
                Span<Endpoints> destCurrentOutEdges =
                    edgesOrderedByTail.AsSpan(currentLowerBound, currentOutEdges.Length);
                currentOutEdges.CopyTo(destCurrentOutEdges);
                destUpperBoundByVertex[vertex] = currentLowerBound + currentOutEdges.Length;
            }

            return new SimpleIncidenceGraph(data, edgesOrderedByTail);
        }

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
