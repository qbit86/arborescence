#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Edge = Endpoints<int>;
    using VertexEnumerator = System.ArraySegment<int>.Enumerator;
    using EdgeEnumerator = IncidenceEnumerator<int, System.ArraySegment<int>.Enumerator>;

    internal readonly struct Int32FrozenAdjacencyGraph :
        IHeadIncidence<int, Edge>,
        ITailIncidence<int, Edge>,
        IOutEdgesIncidence<int, EdgeEnumerator>,
        IAdjacency<int, VertexEnumerator>,
        IEquatable<Int32FrozenAdjacencyGraph>
    {
        public bool TryGetHead(Edge edge, out int head) => throw new NotImplementedException();

        public bool TryGetTail(Edge edge, out int tail) => throw new NotImplementedException();

        public EdgeEnumerator EnumerateOutEdges(int vertex) => throw new NotImplementedException();

        public VertexEnumerator EnumerateOutNeighbors(int vertex) => throw new NotImplementedException();

        public bool Equals(Int32FrozenAdjacencyGraph other) => throw new NotImplementedException();

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32FrozenAdjacencyGraph other && Equals(other);

        public override int GetHashCode() => throw new NotImplementedException();

        public static bool operator ==(Int32FrozenAdjacencyGraph left, Int32FrozenAdjacencyGraph right) =>
            left.Equals(right);

        public static bool operator !=(Int32FrozenAdjacencyGraph left, Int32FrozenAdjacencyGraph right) =>
            !left.Equals(right);
    }
}
#endif
