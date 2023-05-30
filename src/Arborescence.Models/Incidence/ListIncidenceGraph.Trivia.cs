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

        public static bool operator ==(
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> left,
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> right) => left.Equals(right);

        public static bool operator !=(
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> left,
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> right) => !left.Equals(right);
    }
}
