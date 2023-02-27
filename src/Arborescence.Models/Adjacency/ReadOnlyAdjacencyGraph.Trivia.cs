namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy>
    {
        public bool Equals(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> other)
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> self = this;
            return EqualityComparer<TVertexMultimap>.Default.Equals(
                    self._neighborsByVertex, other._neighborsByVertex) &&
                EqualityComparer<TVertexMultimapPolicy>.Default.Equals(
                    self._vertexMultimapPolicy, other._vertexMultimapPolicy);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> other &&
            Equals(other);

        public override int GetHashCode()
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> self = this;
            return HashCode.Combine(EqualityComparer<TVertexMultimap>.Default.GetHashCode(self._neighborsByVertex!),
                EqualityComparer<TVertexMultimapPolicy>.Default.GetHashCode(self._vertexMultimapPolicy));
        }

        public static bool operator ==(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> left,
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> right) =>
            left.Equals(right);

        public static bool operator !=(
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> left,
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> right) =>
            !left.Equals(right);
    }
}
