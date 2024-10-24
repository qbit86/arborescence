namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using static TryHelpers;
    using NeighborEnumerator =
        AdjacencyEnumerator<int, int, Int32IncidenceGraph, System.ArraySegment<int>.Enumerator>;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

    /// <summary>
    /// Provides an efficient implementation of an incidence graph
    /// when the vertices are <see cref="int"/> indices from the contiguous range [0..VertexCount).
    /// </summary>
    public readonly partial struct Int32IncidenceGraph :
        IHeadIncidence<int, int>,
        ITailIncidence<int, int>,
        IOutEdgesIncidence<int, EdgeEnumerator>,
        IOutNeighborsAdjacency<int, NeighborEnumerator>,
        IEquatable<Int32IncidenceGraph>
    {
        // Offset       | Length    | Content
        // -------------|-----------|--------
        // 0            | 1         | n = vertexCount
        // 1            | 1         | m = edgeCount
        // 2            | n         | upperBoundByVertex
        // 2 + n        | m         | edgesOrderedByTail
        // 2 + n + m    | m         | headByEdge
        // 2 + n + 2m   | m         | tailByEdge
        private readonly int[] _data;

        internal Int32IncidenceGraph(int[] data)
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
                var self = this;
                return self._data is null ? 0 : self.VertexCountUnchecked;
            }
        }

        private int VertexCountUnchecked => _data[0];

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount
        {
            get
            {
                var self = this;
                return self._data is null ? 0 : self.EdgeCountUnchecked;
            }
        }

        private int EdgeCountUnchecked => _data[1];

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            var headByEdge = GetHeadByEdge();
            return unchecked((uint)edge < (uint)headByEdge.Length) ? Some(headByEdge[edge], out head) : None(out head);
        }

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail)
        {
            var tailByEdge = GetTailByEdge();
            return unchecked((uint)edge < (uint)tailByEdge.Length) ? Some(tailByEdge[edge], out tail) : None(out tail);
        }

        /// <inheritdoc/>
        public EdgeEnumerator EnumerateOutEdges(int vertex)
        {
            var self = this;
            if (self._data is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            int vertexCount = self.VertexCountUnchecked;
            Debug.Assert(vertexCount >= 0, "vertexCount >= 0");
            if (unchecked((uint)vertex >= (uint)vertexCount))
                return ArraySegment<int>.Empty.GetEnumerator();

            var upperBoundByVertex = self.GetUpperBoundByVertexUnchecked();
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return ArraySegment<int>.Empty.GetEnumerator();

            int lowerBound = vertex == 0 ? 2 + vertexCount : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            int count = upperBound - lowerBound;
            ArraySegment<int> segment = new(self._data, lowerBound, count);
            return segment.GetEnumerator();
        }

        /// <inheritdoc/>
        public NeighborEnumerator EnumerateOutNeighbors(int vertex) =>
            AdjacencyEnumerator<int, EdgeEnumerator>.Create(this, vertex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetHeadByEdge()
        {
            var self = this;
            if (self._data is null)
                return ReadOnlySpan<int>.Empty;
            int n = self.VertexCountUnchecked;
            int m = self.EdgeCountUnchecked;
            return self._data.AsSpan(2 + n + m, m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTailByEdge()
        {
            var self = this;
            if (self._data is null)
                return ReadOnlySpan<int>.Empty;
            int n = self.VertexCountUnchecked;
            int m = self.EdgeCountUnchecked;
            return self._data.AsSpan(2 + n + m + m, m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertexUnchecked()
        {
            var self = this;
            return self._data.AsSpan(2, self.VertexCountUnchecked);
        }
    }
}
