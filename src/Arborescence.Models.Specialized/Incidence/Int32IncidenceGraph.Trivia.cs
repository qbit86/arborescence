namespace Arborescence.Models.Specialized
{
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32IncidenceGraph
    {
        /// <inheritdoc/>
        public bool Equals(Int32IncidenceGraph other) => _data == other._data;

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32IncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int[]? data = _data;
            return data is null ? 0 : data.GetHashCode();
        }

        public static bool operator ==(Int32IncidenceGraph left, Int32IncidenceGraph right) =>
            left.Equals(right);

        public static bool operator !=(Int32IncidenceGraph left, Int32IncidenceGraph right) =>
            !left.Equals(right);
    }
}
