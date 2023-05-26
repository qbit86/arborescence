namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept>
    {
        public bool Equals(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> other)
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return EqualityComparer<TVertexMultimap>.Default.Equals(
                    self._neighborsByVertex, other._neighborsByVertex) &&
                EqualityComparer<TVertexMultimapConcept>.Default.Equals(
                    self._vertexMultimapConcept, other._vertexMultimapConcept);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> other &&
            Equals(other);

        public override int GetHashCode()
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return HashCode.Combine(EqualityComparer<TVertexMultimap>.Default.GetHashCode(self._neighborsByVertex!),
                EqualityComparer<TVertexMultimapConcept>.Default.GetHashCode(self._vertexMultimapConcept));
        }

        public static bool operator ==(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> left,
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> right) =>
            left.Equals(right);

        public static bool operator !=(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> left,
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> right) =>
            !left.Equals(right);
    }
}
