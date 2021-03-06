﻿namespace Arborescence.Models.Compatibility
{
    using System;

    public readonly partial struct SimpleIncidenceGraph
    {
        /// <inheritdoc/>
        public sealed class UndirectedBuilder : IGraphBuilder<SimpleIncidenceGraph, int, Endpoints>
        {
            private int _edgeCount;
            private ArrayPrefix<Endpoints> _edges;
            private int _vertexCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="UndirectedBuilder"/> class.
            /// </summary>
            /// <param name="initialVertexCount">The initial number of vertices.</param>
            /// <param name="edgeCapacity">The initial capacity of the internal backing storage for the edges.</param>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <paramref name="initialVertexCount"/> is less than zero, or <paramref name="edgeCapacity"/> is less than zero.
            /// </exception>
            public UndirectedBuilder(int initialVertexCount = 0, int edgeCapacity = 0)
            {
                if (initialVertexCount < 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(edgeCapacity));

                _edges = ArrayPrefixBuilder.Create<Endpoints>(edgeCapacity);
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
            public bool TryAdd(int tail, int head, out Endpoints edge)
            {
                bool result = tail >= 0 && head >= 0;
                edge = result ? UncheckedAdd(tail, head) : default;
                return result;
            }

            /// <inheritdoc/>
            public SimpleIncidenceGraph ToGraph()
            {
                int n = _vertexCount;
                Endpoints[] array = _edges.Array!;

                Array.Sort(array, 0, _edges.Count, SimpleEdgeComparer.Instance);

                Endpoints[] edgesOrderedByTail;
                if (array.Length == _edges.Count)
                {
                    edgesOrderedByTail = array;
                    _edges = ArrayPrefix<Endpoints>.Empty;
                }
                else
                {
#if NET5_0_OR_GREATER
                    edgesOrderedByTail = GC.AllocateUninitializedArray<Endpoints>(_edges.Count);
#else
                    edgesOrderedByTail = new Endpoints[_edges.Count];
#endif
                    _edges.CopyTo(edgesOrderedByTail);
                    _edges = ArrayPrefixBuilder.Release(_edges, false);
                }

                int[] data = new int[2 + n];
                data[0] = n;
                data[1] = _edgeCount;

                Span<int> upperBoundByVertex = data.AsSpan(2);
                foreach (Endpoints edge in edgesOrderedByTail)
                {
                    int tail = edge.Tail;
                    ++upperBoundByVertex[tail];
                }

                for (int vertex = 1; vertex < n; ++vertex)
                    upperBoundByVertex[vertex] += upperBoundByVertex[vertex - 1];

                _vertexCount = 0;

                return new SimpleIncidenceGraph(data, edgesOrderedByTail);
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
            public Endpoints Add(int tail, int head)
            {
                if (tail < 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(tail));

                if (head < 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(head));

                return UncheckedAdd(tail, head);
            }

            private Endpoints UncheckedAdd(int tail, int head)
            {
                int newVertexCountCandidate = Math.Max(tail, head) + 1;
                if (newVertexCountCandidate > _vertexCount)
                    _vertexCount = newVertexCountCandidate;

                var edge = new Endpoints(tail, head);
                _edges = ArrayPrefixBuilder.Add(_edges, edge, false);
                ++_edgeCount;

                if (tail != head)
                {
                    var invertedEdge = new Endpoints(head, tail);
                    _edges = ArrayPrefixBuilder.Add(_edges, invertedEdge, false);
                }

                return edge;
            }
        }
    }
}
