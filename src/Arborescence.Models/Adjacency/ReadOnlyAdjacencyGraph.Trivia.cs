namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept>
    {
        /// <inheritdoc/>
        public bool Equals(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> other)
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return EqualityComparer<TVertexMultimap>.Default.Equals(
                    self._neighborsByVertex, other._neighborsByVertex) &&
                EqualityComparer<TVertexMultimapConcept>.Default.Equals(
                    self._vertexMultimapConcept, other._vertexMultimapConcept);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> other &&
            Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return HashCode.Combine(EqualityComparer<TVertexMultimap>.Default.GetHashCode(self._neighborsByVertex!),
                EqualityComparer<TVertexMultimapConcept>.Default.GetHashCode(self._vertexMultimapConcept));
        }

        /// <summary>
        /// Indicates whether two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/> structures
        /// are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/> structures
        /// are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> left,
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> right) =>
            left.Equals(right);

        /// <summary>
        /// Indicates whether two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/> structures
        /// are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/> structures
        /// are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> left,
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> right) =>
            !left.Equals(right);
    }
}
