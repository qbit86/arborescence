#if NETSTANDARD2_1 || NETCOREAPP2_1
namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    public readonly partial struct UndirectedIndexedIncidenceGraph
    {
        /// <inheritdoc/>
        public sealed class Builder : IGraphBuilder<UndirectedIndexedIncidenceGraph, int, int>
        {
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
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(edgeCapacity));

                _headByEdge = ArrayPrefixBuilder.Create<int>(edgeCapacity);
                _tailByEdge = ArrayPrefixBuilder.Create<int>(edgeCapacity);
                _vertexCount = initialVertexCount;
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
            public UndirectedIndexedIncidenceGraph ToGraph()
            {
                int n = _vertexCount;
                int m = _tailByEdge.Count;
                Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");

                int dataLength = 2 + n + m + m + m + m;
#if NET5
                int[] data = GC.AllocateUninitializedArray<int>(dataLength);
#else
                int[] data = new int[dataLength];
#endif
                data[0] = n;
                data[1] = m;

                Span<int> destEdgesOrderedByTail = data.AsSpan(2 + n, m + m);
                int invertedEdgeCount = 0;
                for (int edge = 0; edge < m; ++edge)
                {
                    destEdgesOrderedByTail[edge] = edge;
                    int head = _headByEdge[edge];
                    int tail = _tailByEdge[edge];
                    if (head != tail)
                        destEdgesOrderedByTail[m + invertedEdgeCount++] = ~edge;
                }

                int directedEdgeCount = m + invertedEdgeCount;
                Array.Sort(data, 2 + n, directedEdgeCount,
                    new UndirectedIndexedEdgeComparer(_tailByEdge.Array!, _headByEdge.Array!));

                Span<int> destUpperBoundByVertex = data.AsSpan(2, n);
                destUpperBoundByVertex.Clear();
                for (int i = 0; i < directedEdgeCount; ++i)
                {
                    int edge = destEdgesOrderedByTail[i];
                    int tail = edge < 0 ? _headByEdge[~edge] : _tailByEdge[edge];
                    ++destUpperBoundByVertex[tail];
                }

                for (int vertex = 1; vertex < n; ++vertex)
                    destUpperBoundByVertex[vertex] += destUpperBoundByVertex[vertex - 1];

                Span<int> destHeadByEdge = data.AsSpan(2 + n + m + m, m);
                _headByEdge.AsSpan().CopyTo(destHeadByEdge);

                Span<int> destTailByEdge = data.AsSpan(2 + n + m + m + m, m);
                _tailByEdge.AsSpan().CopyTo(destTailByEdge);

                _headByEdge = ArrayPrefixBuilder.Release(_headByEdge, false);
                _tailByEdge = ArrayPrefixBuilder.Release(_tailByEdge, false);
                _vertexCount = 0;

                return new UndirectedIndexedIncidenceGraph(data);
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
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(tail));

                if (head < 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(head));

                return UncheckedAdd(tail, head);
            }

            private int UncheckedAdd(int tail, int head)
            {
                int newVertexCountCandidate = Math.Max(tail, head) + 1;
                if (newVertexCountCandidate > _vertexCount)
                    _vertexCount = newVertexCountCandidate;

                Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");
                int edge = _tailByEdge.Count;
                _tailByEdge = ArrayPrefixBuilder.Add(_tailByEdge, tail, false);
                _headByEdge = ArrayPrefixBuilder.Add(_headByEdge, head, false);

                return edge;
            }
        }
    }
}
#endif
