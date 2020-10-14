#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1
namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    public readonly partial struct SimpleIncidenceGraph
    {
#pragma warning disable CA1034 // Nested types should not be visible
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
            public UndirectedBuilder(int initialVertexCount, int edgeCapacity = 0)
            {
                if (initialVertexCount < 0)
                    throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

                if (edgeCapacity < 0)
                    throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

                _edges = ArrayPrefixBuilder.Create<Endpoints>(edgeCapacity);
                _vertexCount = initialVertexCount;
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
                if (newVertexCountCandidate > _vertexCount)
                    _vertexCount = newVertexCountCandidate;

                _edges = ArrayPrefixBuilder.Add(_edges, edge, false);
                ++_edgeCount;

                if (tail != head)
                {
                    var invertedEdge = new Endpoints(head, tail);
                    _edges = ArrayPrefixBuilder.Add(_edges, invertedEdge, false);
                }

                return true;
            }

            /// <inheritdoc/>
            public SimpleIncidenceGraph ToGraph()
            {
                int n = _vertexCount;
                Endpoints[] array = _edges.Array;
                Debug.Assert(array != null, nameof(array) + " != null");

                Array.Sort(array, 0, _edges.Count, SimpleEdgeComparer.Instance);

                Endpoints[] edgesOrderedByTail;
                if (array.Length == _edges.Count)
                {
                    edgesOrderedByTail = array;
                    _edges = ArrayPrefix<Endpoints>.Empty;
                }
                else
                {
#if NET5
                    edgesOrderedByTail = GC.AllocateUninitializedArray<Endpoints>(_edges.Count);
#else
                    edgesOrderedByTail = new Endpoints[_edges.Count];
#endif
                    _edges.CopyTo(edgesOrderedByTail);
                    _edges = ArrayPrefixBuilder.Release(_edges, false);
                }

                var data = new int[2 + n];
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
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
#endif
