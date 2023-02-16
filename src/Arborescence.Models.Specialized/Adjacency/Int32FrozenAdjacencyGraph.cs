#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VertexEnumerator = System.ArraySegment<int>.Enumerator;

    internal struct Int32IncidenceEnumerator : IEnumerator<Int32Endpoints>
    {
        public bool MoveNext() => throw new NotImplementedException();

        public void Reset() => throw new NotImplementedException();

        public Int32Endpoints Current => throw new NotImplementedException();

        object IEnumerator.Current => Current;

        public void Dispose() => throw new NotImplementedException();
    }

    internal readonly struct Int32FrozenAdjacencyGraph :
        IHeadIncidence<int, Int32Endpoints>,
        ITailIncidence<int, Int32Endpoints>,
        IOutEdgesIncidence<int, Int32IncidenceEnumerator>,
        IAdjacency<int, VertexEnumerator>,
        IEquatable<Int32FrozenAdjacencyGraph>
    {
        public bool TryGetHead(Int32Endpoints edge, out int head) => throw new NotImplementedException();

        public bool TryGetTail(Int32Endpoints edge, out int tail) => throw new NotImplementedException();

        public Int32IncidenceEnumerator EnumerateOutEdges(int vertex) => throw new NotImplementedException();

        public bool Equals(Int32FrozenAdjacencyGraph other) => throw new NotImplementedException();

        public VertexEnumerator EnumerateOutNeighbors(int vertex) => throw new NotImplementedException();

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
