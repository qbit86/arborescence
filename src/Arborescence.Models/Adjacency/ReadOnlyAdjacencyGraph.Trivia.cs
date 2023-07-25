namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept>
    {
        /// <inheritdoc/>
        public bool Equals(
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> other)
        {
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return EqualityComparer<TVertexMultimap>.Default.Equals(
                    self._neighborsByVertex, other._neighborsByVertex) &&
                EqualityComparer<TVertexMultimapConcept>.Default.Equals(
                    self._vertexMultimapConcept, other._vertexMultimapConcept);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept>
                other &&
            Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return HashCode.Combine(EqualityComparer<TVertexMultimap>.Default.GetHashCode(self._neighborsByVertex!),
                EqualityComparer<TVertexMultimapConcept>.Default.GetHashCode(self._vertexMultimapConcept));
        }

        /// <summary>
        /// Indicates whether two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>
        /// structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>
        /// structures are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> left,
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> right) =>
            left.Equals(right);

        /// <summary>
        /// Indicates whether two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>
        /// structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <see langword="true"/> if the two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>
        /// structures are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> left,
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> right) =>
            !left.Equals(right);
    }
}
