namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct SimpleIncidenceGraph
    {
        /// <inheritdoc/>
#pragma warning disable CA1034 // Nested types should not be visible
        public sealed class Builder : IGraphBuilder<SimpleIncidenceGraph, int, uint>
        {
            private int _currentMaxTail;
            private ArrayPrefix<uint> _edges;
            private int _vertexCount;

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

                _edges = ArrayPrefixBuilder.Create<uint>(edgeCapacity);
                _vertexCount = initialVertexCount;
            }

            private bool NeedsReordering => _currentMaxTail == int.MaxValue;

            /// <inheritdoc/>
            public bool TryAdd(int tail, int head, out uint edge)
            {
                if (unchecked((uint)tail) > ushort.MaxValue)
                {
                    edge = default;
                    return false;
                }

                if (unchecked((uint)head) > ushort.MaxValue)
                {
                    edge = default;
                    return false;
                }

                edge = ((uint)tail << 16) | (uint)head;

                _currentMaxTail = tail < _currentMaxTail ? int.MaxValue : tail;

                int newVertexCountCandidate = Math.Max(tail, head) + 1;
                if (newVertexCountCandidate > _vertexCount)
                    _vertexCount = newVertexCountCandidate;

                _edges = ArrayPrefixBuilder.Add(_edges, edge, false);
                return true;
            }

            /// <inheritdoc/>
            public SimpleIncidenceGraph ToGraph()
            {
                int edgeCount = _edges.Count;
                if (NeedsReordering)
                    Array.Sort(_edges.Array, 0, edgeCount, EdgeComparer.Instance);

                int storageLength = 1 + _vertexCount + edgeCount;
#if NET5
                uint[] storage = GC.AllocateUninitializedArray<uint>(storageLength);
#else
                var storage = new uint[storageLength];
#endif
                storage[0] = (uint)_vertexCount;
                // TODO: Test for empty _edges (when no calls to TryAdd were performed).
                _edges.CopyTo(storage, 1 + _vertexCount);
                Span<uint> upperBoundByVertex = storage.AsSpan(1, _vertexCount);
                upperBoundByVertex.Fill(0u);
                for (int edgeIndex = 0; edgeIndex < edgeCount; ++edgeIndex)
                {
                    uint edge = _edges[edgeIndex];
                    int tail = (int)(edge >> 16);
                    ++upperBoundByVertex[tail];
                }

                for (int vertex = 1; vertex < _vertexCount; ++vertex)
                    upperBoundByVertex[vertex] += upperBoundByVertex[vertex - 1];

                _currentMaxTail = 0;
                _edges = ArrayPrefixBuilder.Dispose(_edges, false);
                _vertexCount = 0;

                return new SimpleIncidenceGraph(storage);
            }
        }

        internal sealed class EdgeComparer : IComparer<uint>
        {
            public static EdgeComparer Instance { get; } = new EdgeComparer();

            public int Compare(uint x, uint y)
            {
                const uint mask = 0xFFFF0000u;
                uint left = x & mask;
                uint right = y & mask;
                return left.CompareTo(right);
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
