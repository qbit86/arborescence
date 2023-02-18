#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Incidence
{
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32FrozenIncidenceGraph
    {
        public bool Equals(Int32FrozenIncidenceGraph other) => _data == other._data;

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32FrozenIncidenceGraph other && Equals(other);

        public override int GetHashCode()
        {
            int[]? data = _data;
            return data is null ? 0 : data.GetHashCode();
        }

        public static bool operator ==(Int32FrozenIncidenceGraph left, Int32FrozenIncidenceGraph right) =>
            left.Equals(right);

        public static bool operator !=(Int32FrozenIncidenceGraph left, Int32FrozenIncidenceGraph right) =>
            !left.Equals(right);
    }
}
#endif
