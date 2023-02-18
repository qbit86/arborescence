#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Incidence
{
    using System;
    using System.Diagnostics;
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
        private readonly int[] _data;

        internal Int32FrozenIncidenceGraph(int[] data)
        {
            Debug.Assert(data is not null, "data is not null");
            _data = data;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _data is null ? 0 : VertexCountUnchecked;

        private int VertexCountUnchecked => _data[0];

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => _data is null ? 0 : EdgeCountUnchecked;

        private int EdgeCountUnchecked => _data[1];

        public bool TryGetHead(int edge, out int head) => throw new NotImplementedException();

        public bool TryGetTail(int edge, out int tail) => throw new NotImplementedException();

        public EdgeEnumerator EnumerateOutEdges(int vertex) => throw new NotImplementedException();

        public VertexEnumerator EnumerateOutNeighbors(int vertex) => throw new NotImplementedException();
    }
}
#endif
