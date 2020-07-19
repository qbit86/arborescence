namespace Arborescence.Models
{
    using System;
    using System.Buffers;
    using System.Diagnostics;

    public readonly partial struct UndirectedIndexedIncidenceGraph
    {
#pragma warning disable CA1034 // Nested types should not be visible
        /// <inheritdoc/>
        public sealed class Builder : IGraphBuilder<UndirectedIndexedIncidenceGraph, int, int>
        {
            private const int DefaultInitialOutDegree = 8;

            private ArrayBuilder<int> _heads;
            private int _initialOutDegree = DefaultInitialOutDegree;
            private ArrayPrefix<ArrayBuilder<int>> _outEdges;
            private ArrayBuilder<int> _tails;

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            /// <param name="initialVertexCount">The initial number of vertices.</param>
            /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
            /// </exception>
            public Builder(int initialVertexCount, int edgeCapacity = 0)
            {
                if (initialVertexCount < 0)
                    throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

                int effectiveEdgeCapacity = Math.Max(2 * edgeCapacity, DefaultInitialOutDegree);
                _tails = new ArrayBuilder<int>(effectiveEdgeCapacity);
                _heads = new ArrayBuilder<int>(effectiveEdgeCapacity);
                ArrayBuilder<int>[] outEdges = Pool.Rent(initialVertexCount);
                Array.Clear(outEdges, 0, initialVertexCount);
                _outEdges = ArrayPrefix.Create(outEdges, initialVertexCount);
            }

            private static ArrayPool<ArrayBuilder<int>> Pool => ArrayPool<ArrayBuilder<int>>.Shared;

            /// <summary>
            /// Gets the number of vertices.
            /// </summary>
            public int VertexCount => _outEdges.Count;

            /// <summary>
            /// Gets the initial number of out-edges for each vertex.
            /// </summary>
            public int InitialOutDegree
            {
                get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
                set => _initialOutDegree = 2 * value;
            }

            /// <inheritdoc/>
            public bool TryAdd(int tail, int head, out int edge)
            {
                if (tail < 0)
                {
                    edge = default;
                    return false;
                }

                if (head < 0)
                {
                    edge = default;
                    return false;
                }

                int max = Math.Max(tail, head);
                EnsureVertexCount(max + 1);

                Debug.Assert(_tails.Count == _heads.Count);
                int newEdgeIndex = _heads.Count;
                _tails.Add(tail);
                _heads.Add(head);

                if (_outEdges[tail].Buffer == null)
                    _outEdges[tail] = new ArrayBuilder<int>(InitialOutDegree);

                _outEdges.Array[tail].Add(newEdgeIndex);
                _outEdges.Array[head].Add(~newEdgeIndex);

                edge = newEdgeIndex;
                return true;
            }

            /// <inheritdoc/>
            public UndirectedIndexedIncidenceGraph ToGraph()
            {
                Debug.Assert(_tails.Count == _heads.Count);
                int vertexCount = VertexCount;
                int tailCount = _tails.Count;
                int headCount = _heads.Count;
                int reorderedEdgeCount = 2 * tailCount;
                var storage = new int[1 + 2 * vertexCount + reorderedEdgeCount + headCount + tailCount];
                storage[0] = vertexCount;

                Span<ArrayBuilder<int>> outEdges = _outEdges.AsSpan();
                Span<int> destEdgeBounds = storage.AsSpan(1, 2 * vertexCount);
                Span<int> destReorderedEdges = storage.AsSpan(1 + 2 * vertexCount, reorderedEdgeCount);

                for (int s = 0, currentBound = 0; s < outEdges.Length; ++s)
                {
                    ReadOnlySpan<int> currentOutEdges = outEdges[s].AsSpan();
                    currentOutEdges.CopyTo(destReorderedEdges.Slice(currentBound, currentOutEdges.Length));
                    int finalLeftBound = 1 + 2 * vertexCount + currentBound;
                    destEdgeBounds[2 * s] = finalLeftBound;
                    destEdgeBounds[2 * s + 1] = currentOutEdges.Length;
                    currentBound += currentOutEdges.Length;
                    outEdges[s].Dispose(false);
                }

                if (_outEdges.Array != null)
                    Pool.Return(_outEdges.Array, true);
                _outEdges = ArrayPrefix<ArrayBuilder<int>>.Empty;

                Span<int> destHeads = storage.AsSpan(1 + 2 * vertexCount + reorderedEdgeCount, headCount);
                _heads.AsSpan().CopyTo(destHeads);
                _heads.Dispose(false);

                Span<int> destTails = storage.AsSpan(1 + 2 * vertexCount + reorderedEdgeCount + headCount, tailCount);
                _tails.AsSpan().CopyTo(destTails);
                _tails.Dispose(false);

                return new UndirectedIndexedIncidenceGraph(storage);
            }

            /// <summary>
            /// Ensures that the builder can hold the specified number of vertices without growing.
            /// </summary>
            /// <param name="vertexCount">The number of vertices.</param>
            public void EnsureVertexCount(int vertexCount)
            {
                if (vertexCount > VertexCount)
                    _outEdges = ArrayPrefixBuilder.Resize(_outEdges, vertexCount, true);
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
