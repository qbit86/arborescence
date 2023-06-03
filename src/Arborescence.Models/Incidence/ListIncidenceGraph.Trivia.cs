namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    partial struct ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>
    {
        /// <inheritdoc/>
        public bool Equals(ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> other)
        {
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            return EqualityComparer<TEndpointMap>.Default.Equals(self._tailByEdge, other._tailByEdge) &&
                EqualityComparer<TEndpointMap>.Default.Equals(self._headByEdge, other._headByEdge) &&
                EqualityComparer<TEdgeMultimap>.Default.Equals(self._outEdgesByVertex, other._outEdgesByVertex);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            return HashCode.Combine(
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._tailByEdge),
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._headByEdge),
                EqualityComparer<TEdgeMultimap>.Default.GetHashCode(self._outEdgesByVertex));
        }

        /// <summary>
        /// Indicates whether two <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>
        /// structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two
        /// <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>
        /// structures are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> left,
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>
        /// structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two
        /// <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>
        /// structures are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> left,
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> right) => !left.Equals(right);
    }
}
