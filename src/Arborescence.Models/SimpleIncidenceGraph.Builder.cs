namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct SimpleIncidenceGraph
    {
#pragma warning disable CA1034 // Nested types should not be visible
        /// <inheritdoc/>
        public sealed class Builder : IGraphBuilder<SimpleIncidenceGraph, int, Endpoints>
        {
            private int _currentMaxTail;
            private ArrayPrefix<Endpoints> _edges;
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

                _edges = ArrayPrefixBuilder.Create<Endpoints>(edgeCapacity);
                _vertexCount = initialVertexCount;
            }

            private bool NeedsReordering => _currentMaxTail == int.MaxValue;

            /// <inheritdoc/>
            public bool TryAdd(int tail, int head, out Endpoints edge)
            {
                edge = new Endpoints(tail, head);
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
                int m = _edges.Count;
                if (NeedsReordering)
                    Array.Sort(_edges.Array, 0, m, EdgeComparer.Instance);

#if NET5
                Endpoints[] edgesOrderedByTail = GC.AllocateUninitializedArray<Endpoints>(m);
#else
                var edgesOrderedByTail = new Endpoints[m];
#endif
#if false
                _edges.CopyTo(edgesOrderedByTail);
                Span<uint> upperBoundByVertex = storage.AsSpan(1, _vertexCount);
                upperBoundByVertex.Clear();
                for (int edgeIndex = 0; edgeIndex < edgeCount; ++edgeIndex)
                {
                    uint edge = _edges[edgeIndex];
                    int tail = (int)(edge >> 16);
                    ++upperBoundByVertex[tail];
                }

                for (int vertex = 1; vertex < _vertexCount; ++vertex)
                    upperBoundByVertex[vertex] += upperBoundByVertex[vertex - 1];

                _currentMaxTail = 0;
                _edges = ArrayPrefixBuilder.Release(_edges, false);
                _vertexCount = 0;

                return new SimpleIncidenceGraph(storage);
#else
                throw new NotImplementedException();
#endif
            }
        }

        internal sealed class EdgeComparer : IComparer<Endpoints>
        {
            public static EdgeComparer Instance { get; } = new EdgeComparer();

            public int Compare(Endpoints x, Endpoints y)
            {
                int left = x.Tail;
                int right = y.Tail;
                return left.CompareTo(right);
            }
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
