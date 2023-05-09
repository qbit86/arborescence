#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Edge = Endpoints<int>;
    using VertexEnumerator = System.ArraySegment<int>.Enumerator;
    using EdgeEnumerator = IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>;

    public readonly partial struct Int32AdjacencyGraph :
        IHeadIncidence<int, Edge>,
        ITailIncidence<int, Edge>,
        IOutEdgesIncidence<int, EdgeEnumerator>,
        IOutNeighborsAdjacency<int, VertexEnumerator>,
        IEquatable<Int32AdjacencyGraph>
    {
        // Offset       | Length    | Content
        // -------------|-----------|--------
        // 0            | 1         | n = vertexCount
        // 1            | 1         | m = edgeCount
        // 2            | n         | upperBoundByVertex
        // 2 + n        | m         | neighborsOrderedByTail
        private readonly int[] _data;

        internal Int32AdjacencyGraph(int[] data)
        {
            Debug.Assert(data is not null, "data is not null");
            _data = data;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount
        {
            get
            {
                Int32AdjacencyGraph self = this;
                return self._data is null ? 0 : self.VertexCountUnchecked;
            }
        }

        private int VertexCountUnchecked => _data[0];

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => (_data?[1]).GetValueOrDefault();

        public bool TryGetHead(Edge edge, out int head)
        {
            head = edge.Head;
            return unchecked((uint)head < (uint)VertexCount);
        }

        public bool TryGetTail(Edge edge, out int tail)
        {
            tail = edge.Tail;
            return unchecked((uint)tail < (uint)VertexCount);
        }

        public EdgeEnumerator EnumerateOutEdges(int vertex)
        {
            VertexEnumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return IncidenceEnumerator.Create(vertex, neighborEnumerator);
        }

        public VertexEnumerator EnumerateOutNeighbors(int vertex)
        {
            Int32AdjacencyGraph self = this;
            if (self._data is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            int vertexCount = self.VertexCountUnchecked;
            Debug.Assert(vertexCount >= 0, "vertexCount >= 0");
            if (unchecked((uint)vertex >= (uint)vertexCount))
                return ArraySegment<int>.Empty.GetEnumerator();

            ReadOnlySpan<int> upperBoundByVertex = self.GetUpperBoundByVertexUnchecked();
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return ArraySegment<int>.Empty.GetEnumerator();

            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            int count = upperBound - lowerBound;
            ArraySegment<int> segment = new(self._data, lowerBound, count);
            return segment.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertexUnchecked()
        {
            Int32AdjacencyGraph self = this;
            Debug.Assert(self._data.Length >= 2, "_data.Length >= 2");
            return self._data.AsSpan(2, self.VertexCountUnchecked);
        }
    }
}
#endif
