#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    internal readonly partial struct Int32FrozenAdjacencyGraph
    {
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
