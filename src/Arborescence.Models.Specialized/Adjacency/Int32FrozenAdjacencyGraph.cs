#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics;
    using static TryHelpers;
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

        public bool TryGetHead(Edge edge, out int head) => Some(edge.Head, out head);

        public bool TryGetTail(Edge edge, out int tail) => Some(edge.Tail, out tail);

        public EdgeEnumerator EnumerateOutEdges(int vertex)
        {
            VertexEnumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return IncidenceEnumerator.Create(vertex, neighborEnumerator);
        }

        public VertexEnumerator EnumerateOutNeighbors(int vertex)
        {
            if (_data is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            Debug.Assert(_data.Length >= 2, "_data.Length >= 2");
            int vertexCount = _data[0];
            Debug.Assert(vertexCount >= 0, "vertexCount >= 0");
            if (unchecked((uint)vertex >= (uint)vertexCount))
                return ArraySegment<int>.Empty.GetEnumerator();

            int edgeCount = _data[1];
            Debug.Assert(edgeCount >= 0, "edgeCount >= 0");
            ReadOnlySpan<int> upperBoundByVertex = _data.AsSpan(2, vertexCount);
            int upperBound = upperBoundByVertex[vertex];
            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int offset = 2 + vertexCount + lowerBound;
            int count = upperBound - lowerBound;
            ArraySegment<int> segment = new(_data, offset, count);
            return segment.GetEnumerator();
        }
    }
}
#endif
