namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct ReadOnlyIncidenceGraph<
        TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>
    {
        public bool Equals(ReadOnlyIncidenceGraph<
            TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> other)
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> self = this;
            return EqualityComparer<TEndpointMap>.Default.Equals(self._tailByEdge, other._tailByEdge) &&
                EqualityComparer<TEndpointMap>.Default.Equals(self._headByEdge, other._headByEdge) &&
                EqualityComparer<TEdgeMultimap>.Default.Equals(self._outEdgesByVertex, other._outEdgesByVertex) &&
                EqualityComparer<TEdgeMultimapPolicy>.Default.Equals(
                    self._edgeMultimapPolicy, other._edgeMultimapPolicy);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> other &&
            Equals(other);

        public override int GetHashCode()
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> self = this;
            return HashCode.Combine(
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._tailByEdge),
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._headByEdge),
                EqualityComparer<TEdgeMultimap>.Default.GetHashCode(self._outEdgesByVertex!),
                EqualityComparer<TEdgeMultimapPolicy>.Default.GetHashCode(self._edgeMultimapPolicy));
        }

        public static bool operator ==(
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> left,
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> right) =>
            left.Equals(right);

        public static bool operator !=(
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> left,
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> right) =>
            !left.Equals(right);
    }
}
