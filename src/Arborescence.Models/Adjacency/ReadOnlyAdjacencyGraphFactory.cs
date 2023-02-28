namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy>
            Create<TVertexMultimap, TVertexMultimapPolicy>(
                TVertexMultimap neighborsByVertex, TVertexMultimapPolicy vertexMultimapPolicy)
            where TVertexMultimapPolicy : IReadOnlyMultimapPolicy<TVertex, TVertexMultimap, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));
            if (vertexMultimapPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vertexMultimapPolicy));

            return new(neighborsByVertex, vertexMultimapPolicy);
        }
    }

    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexCollection, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, ReadOnlyMultimapPolicy<
                TVertex, TVertexCollection, TVertexEnumerator, TVertexMultimap, TVertexCollectionPolicy>>
            Create<TVertexMultimap, TVertexCollectionPolicy>(
                TVertexMultimap neighborsByVertex, TVertexCollectionPolicy vertexCollectionPolicy)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, TVertexCollection>
            where TVertexCollectionPolicy : IEnumerablePolicy<TVertexCollection, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));
            if (vertexCollectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vertexCollectionPolicy));

            ReadOnlyMultimapPolicy<
                    TVertex, TVertexCollection, TVertexEnumerator, TVertexMultimap, TVertexCollectionPolicy>
                vertexMultimapPolicy = new(vertexCollectionPolicy);
            return new(neighborsByVertex, vertexMultimapPolicy);
        }
    }
}
