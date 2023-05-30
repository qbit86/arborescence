namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct ReadOnlyIncidenceGraph<
        TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>
    {
        /// <inheritdoc/>
        public bool Equals(ReadOnlyIncidenceGraph<
            TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> other)
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> self = this;
            return EqualityComparer<TEndpointMap>.Default.Equals(self._tailByEdge, other._tailByEdge) &&
                EqualityComparer<TEndpointMap>.Default.Equals(self._headByEdge, other._headByEdge) &&
                EqualityComparer<TEdgeMultimap>.Default.Equals(self._outEdgesByVertex, other._outEdgesByVertex) &&
                EqualityComparer<TEdgeMultimapConcept>.Default.Equals(
                    self._edgeMultimapConcept, other._edgeMultimapConcept);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> other &&
            Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> self = this;
            return HashCode.Combine(
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._tailByEdge),
                EqualityComparer<TEndpointMap>.Default.GetHashCode(self._headByEdge),
                EqualityComparer<TEdgeMultimap>.Default.GetHashCode(self._outEdgesByVertex!),
                EqualityComparer<TEdgeMultimapConcept>.Default.GetHashCode(self._edgeMultimapConcept));
        }

        public static bool operator ==(
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> left,
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> right) =>
            left.Equals(right);

        public static bool operator !=(
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> left,
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> right) =>
            !left.Equals(right);
    }
}
