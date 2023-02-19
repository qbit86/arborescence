namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    partial struct IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>
    {
        public bool Equals(IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> other)
        {
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            return EqualityComparer<TEndpointMap>.Default.Equals(self._tailByEdge, other._tailByEdge) &&
                EqualityComparer<TEndpointMap>.Default.Equals(self._headByEdge, other._headByEdge) &&
                EqualityComparer<TEdgeMultimap>.Default.Equals(self._outEdgesByVertex, other._outEdgesByVertex);
        }

        public override bool Equals(object? obj) =>
            obj is IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> other && Equals(other);

        public override int GetHashCode()
        {
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            return HashCode.Combine(
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._tailByEdge),
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._headByEdge),
                EqualityComparer<TEdgeMultimap>.Default.GetHashCode(self._outEdgesByVertex));
        }

        public static bool operator ==(
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> left,
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> right) => left.Equals(right);

        public static bool operator !=(
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> left,
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> right) => !left.Equals(right);
    }
}
