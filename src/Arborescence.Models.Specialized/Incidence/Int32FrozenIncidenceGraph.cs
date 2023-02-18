#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Incidence
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using static TryHelpers;
    using VertexEnumerator =
        AdjacencyEnumerator<int, int, Int32FrozenIncidenceGraph, System.ArraySegment<int>.Enumerator>;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

    public readonly partial struct Int32FrozenIncidenceGraph :
        IHeadIncidence<int, int>,
        ITailIncidence<int, int>,
        IOutEdgesIncidence<int, EdgeEnumerator>,
        IAdjacency<int, VertexEnumerator>,
        IEquatable<Int32FrozenIncidenceGraph>
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

        internal Int32FrozenIncidenceGraph(int[] data)
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
                Int32FrozenIncidenceGraph self = this;
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
                Int32FrozenIncidenceGraph self = this;
                return self._data is null ? 0 : self.EdgeCountUnchecked;
            }
        }

        private int EdgeCountUnchecked => _data[1];

        public bool TryGetHead(int edge, out int head)
        {
            ReadOnlySpan<int> headByEdge = GetHeadByEdge();
            return unchecked((uint)edge < (uint)headByEdge.Length) ? Some(headByEdge[edge], out head) : None(out head);
        }

        public bool TryGetTail(int edge, out int tail)
        {
            ReadOnlySpan<int> tailByEdge = GetTailByEdge();
            return unchecked((uint)edge < (uint)tailByEdge.Length) ? Some(tailByEdge[edge], out tail) : None(out tail);
        }

        public EdgeEnumerator EnumerateOutEdges(int vertex)
        {
            Int32FrozenIncidenceGraph self = this;
            if (self._data is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            throw new NotImplementedException();
        }

        public VertexEnumerator EnumerateOutNeighbors(int vertex)
        {
            Int32FrozenIncidenceGraph self = this;
            EdgeEnumerator edgeEnumerator = self.EnumerateOutEdges(vertex);
            return AdjacencyEnumerator<int, int>.Create(self, edgeEnumerator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetHeadByEdge()
        {
            Int32FrozenIncidenceGraph self = this;
            if (self._data is null)
                return ReadOnlySpan<int>.Empty;
            int n = self.VertexCountUnchecked;
            int m = self.EdgeCountUnchecked;
            return self._data.AsSpan(2 + n + m, m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTailByEdge()
        {
            Int32FrozenIncidenceGraph self = this;
            if (self._data is null)
                return ReadOnlySpan<int>.Empty;
            int n = self.VertexCountUnchecked;
            int m = self.EdgeCountUnchecked;
            return self._data.AsSpan(2 + n + m + m, m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertexUnchecked()
        {
            Int32FrozenIncidenceGraph self = this;
            return self._data.AsSpan(2, self.VertexCountUnchecked);
        }
    }
}
#endif
