namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    partial struct AdjacencyGraph<TVertex, TVertexMultimap>
    {
        public bool Equals(AdjacencyGraph<TVertex, TVertexMultimap> other) =>
            EqualityComparer<TVertexMultimap>.Default.Equals(_neighborsByVertex, other._neighborsByVertex);

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is AdjacencyGraph<TVertex, TVertexMultimap> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<TVertexMultimap>.Default.GetHashCode(_neighborsByVertex);

        public static bool operator ==(AdjacencyGraph<TVertex, TVertexMultimap> left,
            AdjacencyGraph<TVertex, TVertexMultimap> right) => left.Equals(right);

        public static bool operator !=(AdjacencyGraph<TVertex, TVertexMultimap> left,
            AdjacencyGraph<TVertex, TVertexMultimap> right) => !left.Equals(right);
    }
}
