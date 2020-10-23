namespace Arborescence.Models.Compatibility
{
    using System;
    using System.Diagnostics;

    public readonly partial struct IndexedIncidenceGraph
    {
#pragma warning disable CA1034 // Nested types should not be visible
        /// <inheritdoc/>
        public sealed class Builder : IGraphBuilder<IndexedIncidenceGraph, int, int>
        {
            private int _currentMaxTail;
            private ArrayPrefix<int> _headByEdge;
            private ArrayPrefix<int> _tailByEdge;
            private int _vertexCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            /// <param name="initialVertexCount">The initial number of vertices.</param>
            /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
            /// </exception>
            public Builder(int initialVertexCount = 0, int edgeCapacity = 0)
            {
                if (initialVertexCount < 0)
                    throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

                _headByEdge = ArrayPrefixBuilder.Create<int>(edgeCapacity);
                _tailByEdge = ArrayPrefixBuilder.Create<int>(edgeCapacity);
                _vertexCount = initialVertexCount;
            }

            private bool NeedsReordering => _currentMaxTail == int.MaxValue;

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
                int n = _vertexCount;
                int m = _tailByEdge.Count;
                Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");

                int dataLength = 2 + n + m + m + m;
#if NET5
                int[] data = GC.AllocateUninitializedArray<int>(dataLength);
#else
                var data = new int[dataLength];
#endif
                data[0] = n;
                data[1] = m;

                Span<int> destReorderedEdges = data.AsSpan(2 + n, m);
                for (int edge = 0; edge < m; ++edge)
                    destReorderedEdges[edge] = edge;

                if (NeedsReordering)
                    Array.Sort(data, 2 + n, m, new IndexedEdgeComparer(_tailByEdge.Array));

                Span<int> destUpperBoundByVertex = data.AsSpan(2, n);
                destUpperBoundByVertex.Clear();
                for (int edge = 0; edge < m; ++edge)
                {
                    int tail = _tailByEdge[edge];
                    ++destUpperBoundByVertex[tail];
                }

                for (int vertex = 1; vertex < n; ++vertex)
                    destUpperBoundByVertex[vertex] += destUpperBoundByVertex[vertex - 1];

                Span<int> destHeadByEdge = data.AsSpan(2 + n + m, m);
                _headByEdge.AsSpan().CopyTo(destHeadByEdge);

                Span<int> destTailByEdge = data.AsSpan(2 + n + m + m, m);
                _tailByEdge.AsSpan().CopyTo(destTailByEdge);

                _currentMaxTail = 0;
                _headByEdge = ArrayPrefixBuilder.Release(_headByEdge, false);
                _tailByEdge = ArrayPrefixBuilder.Release(_tailByEdge, false);
                _vertexCount = 0;

                return new IndexedIncidenceGraph(data);
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
                int edge = _tailByEdge.Count;

                _currentMaxTail = tail < _currentMaxTail ? int.MaxValue : tail;

                int newVertexCountCandidate = Math.Max(tail, head) + 1;
                if (newVertexCountCandidate > _vertexCount)
                    _vertexCount = newVertexCountCandidate;

                Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");
                _tailByEdge = ArrayPrefixBuilder.Add(_tailByEdge, tail, false);
                _headByEdge = ArrayPrefixBuilder.Add(_headByEdge, head, false);

                return edge;
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
