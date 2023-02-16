#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics;
    using Edge = Endpoints<int>;
    using VertexEnumerator = System.ArraySegment<int>.Enumerator;
    using EdgeEnumerator = IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>;

    internal readonly partial struct Int32FrozenAdjacencyGraph :
        IHeadIncidence<int, Edge>,
        ITailIncidence<int, Edge>,
        IOutEdgesIncidence<int, EdgeEnumerator>,
        IAdjacency<int, VertexEnumerator>,
        IEquatable<Int32FrozenAdjacencyGraph>
    {
        private readonly int[] _data;

        internal Int32FrozenAdjacencyGraph(int[] data)
        {
            Debug.Assert(data is not null, "data is not null");
            _data = data;
        }

        public bool TryGetHead(Edge edge, out int head) => throw new NotImplementedException();

        public bool TryGetTail(Edge edge, out int tail) => throw new NotImplementedException();

        public EdgeEnumerator EnumerateOutEdges(int vertex) => throw new NotImplementedException();

        public VertexEnumerator EnumerateOutNeighbors(int vertex) => throw new NotImplementedException();
    }
}
#endif
