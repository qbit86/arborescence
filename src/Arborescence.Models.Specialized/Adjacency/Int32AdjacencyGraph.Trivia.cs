namespace Arborescence.Models.Specialized
{
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32AdjacencyGraph
    {
        public bool Equals(Int32AdjacencyGraph other) => _data == other._data;

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32AdjacencyGraph other && Equals(other);

        public override int GetHashCode()
        {
            int[]? data = _data;
            return data is null ? 0 : data.GetHashCode();
        }

        public static bool operator ==(Int32AdjacencyGraph left, Int32AdjacencyGraph right) =>
            left.Equals(right);

        public static bool operator !=(Int32AdjacencyGraph left, Int32AdjacencyGraph right) =>
            !left.Equals(right);
    }
}
