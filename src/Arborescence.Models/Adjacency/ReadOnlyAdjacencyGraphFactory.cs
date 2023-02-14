namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexCollection, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static ReadOnlyAdjacencyGraph<
                TVertex, TVerticesMap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
            Create<TVerticesMap, TVertexCollectionPolicy>(
                TVerticesMap neighborsByVertex, TVertexCollectionPolicy vertexCollectionPolicy)
            where TVerticesMap : IReadOnlyDictionary<TVertex, TVertexCollection>
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
