namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct AdjacencyGraph<
        TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
    {
        public bool Equals(AdjacencyGraph<
            TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy> other)
        {
            AdjacencyGraph<TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
                self = this;
            return EqualityComparer<TVertexMultimap>.Default.Equals(
                    self._neighborsByVertex, other._neighborsByVertex) &&
                EqualityComparer<TVertexCollectionPolicy>.Default.Equals(
                    self._vertexCollectionPolicy, other._vertexCollectionPolicy);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is AdjacencyGraph<
                TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy> other &&
            Equals(other);

        public override int GetHashCode()
        {
            AdjacencyGraph<TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
                self = this;
            return HashCode.Combine(
                EqualityComparer<TVertexMultimap>.Default.GetHashCode(self._neighborsByVertex),
                EqualityComparer<TVertexCollectionPolicy>.Default.GetHashCode(self._vertexCollectionPolicy));
        }

        public static bool operator ==(
            AdjacencyGraph<TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
                left,
            AdjacencyGraph<TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
                right) =>
            left.Equals(right);

        public static bool operator !=(
            AdjacencyGraph<TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
                left,
            AdjacencyGraph<TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
                right) =>
            !left.Equals(right);
    }
}
