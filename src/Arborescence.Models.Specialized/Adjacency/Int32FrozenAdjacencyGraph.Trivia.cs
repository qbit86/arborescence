#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32FrozenAdjacencyGraph
    {
        public bool Equals(Int32FrozenAdjacencyGraph other) => _data == other._data;

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32FrozenAdjacencyGraph other && Equals(other);

        public override int GetHashCode() => _data is null ? 0 : _data.GetHashCode();

        public static bool operator ==(Int32FrozenAdjacencyGraph left, Int32FrozenAdjacencyGraph right) =>
            left.Equals(right);

        public static bool operator !=(Int32FrozenAdjacencyGraph left, Int32FrozenAdjacencyGraph right) =>
            !left.Equals(right);
    }
}
#endif
