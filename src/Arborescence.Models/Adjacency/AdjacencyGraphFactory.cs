namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class AdjacencyGraphFactory<TVertex, TVertexCollection, TVertexEnumerator>
        where TVertexCollection : ICollection<TVertex>, new()
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static AdjacencyGraph<
                TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
            Create<TVertexMultimap, TVertexCollectionPolicy>(
                TVertexMultimap neighborsByVertex, TVertexCollectionPolicy vertexCollectionPolicy)
            where TVertexMultimap :
            IDictionary<TVertex, TVertexCollection>, IReadOnlyDictionary<TVertex, TVertexCollection>
            where TVertexCollectionPolicy : IEnumerablePolicy<TVertexCollection, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));

            if (vertexCollectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vertexCollectionPolicy));

            return new(neighborsByVertex, vertexCollectionPolicy);
        }
    }
}
