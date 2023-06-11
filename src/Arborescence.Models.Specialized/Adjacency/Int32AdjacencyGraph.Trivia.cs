namespace Arborescence.Models.Specialized
{
    using System.Diagnostics.CodeAnalysis;

    partial struct Int32AdjacencyGraph
    {
        /// <inheritdoc/>
        public bool Equals(Int32AdjacencyGraph other) => _data == other._data;

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32AdjacencyGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int[]? data = _data;
            return data is null ? 0 : data.GetHashCode();
        }

        /// <summary>
        /// Indicates whether two <see cref="Int32AdjacencyGraph"/> structures are equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two <see cref="Int32AdjacencyGraph"/> structures are equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator ==(Int32AdjacencyGraph left, Int32AdjacencyGraph right) =>
            left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="Int32AdjacencyGraph"/> structures are not equal.
        /// </summary>
        /// <param name="left">The instance to the left of the operator.</param>
        /// <param name="right">The instance to the right of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two <see cref="Int32AdjacencyGraph"/> structures are not equal;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator !=(Int32AdjacencyGraph left, Int32AdjacencyGraph right) =>
            !left.Equals(right);
    }
}
