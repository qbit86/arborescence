namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct ListAdjacencyGraph<TVertex, TVertexMultimap>
    {
        /// <inheritdoc/>
        public bool Equals(ListAdjacencyGraph<TVertex, TVertexMultimap> other) =>
            EqualityComparer<TVertexMultimap>.Default.Equals(_neighborsByVertex, other._neighborsByVertex);

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ListAdjacencyGraph<TVertex, TVertexMultimap> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => EqualityComparer<TVertexMultimap>.Default.GetHashCode(_neighborsByVertex);

        /// <summary>
        /// Indicates whether two <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// structures are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(ListAdjacencyGraph<TVertex, TVertexMultimap> left,
            ListAdjacencyGraph<TVertex, TVertexMultimap> right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// structures are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(ListAdjacencyGraph<TVertex, TVertexMultimap> left,
            ListAdjacencyGraph<TVertex, TVertexMultimap> right) => !left.Equals(right);
    }
}
