namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept>
            Create<TVertexMultimap, TVertexMultimapConcept>(
                TVertexMultimap neighborsByVertex, TVertexMultimapConcept vertexMultimapPolicy)
            where TVertexMultimapConcept : IReadOnlyMultimapConcept<TVertex, TVertexMultimap, TVertexEnumerator>
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
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, ReadOnlyMultimapConcept<
                TVertex, TVertexCollection, TVertexEnumerator, TVertexMultimap, TVertexCollectionPolicy>>
            Create<TVertexMultimap, TVertexCollectionPolicy>(
                TVertexMultimap neighborsByVertex, TVertexCollectionPolicy vertexCollectionPolicy)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, TVertexCollection>
            where TVertexCollectionPolicy : IEnumeratorProvider<TVertexCollection, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));
            if (vertexCollectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vertexCollectionPolicy));

            ReadOnlyMultimapConcept<
                    TVertex, TVertexCollection, TVertexEnumerator, TVertexMultimap, TVertexCollectionPolicy>
                vertexMultimapConcept = new(vertexCollectionPolicy);
            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }
}
