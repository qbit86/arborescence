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

        /// <summary>
        /// Indicates whether two <see cref="Int32IncidenceGraph"/> structures are equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two <see cref="Int32IncidenceGraph"/> structures are equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(Int32IncidenceGraph left, Int32IncidenceGraph right) =>
            left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="Int32IncidenceGraph"/> structures are not equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two <see cref="Int32IncidenceGraph"/> structures are not equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(Int32IncidenceGraph left, Int32IncidenceGraph right) =>
            !left.Equals(right);
    }
}
