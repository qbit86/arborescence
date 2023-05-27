namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept>
            Create<TVertexMultimap, TVertexMultimapConcept>(
                TVertexMultimap neighborsByVertex, TVertexMultimapConcept vertexMultimapConcept)
            where TVertexMultimapConcept : IReadOnlyMultimapConcept<TVertexMultimap, TVertex, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));
            if (vertexMultimapConcept is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vertexMultimapConcept));

            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }

    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexCollection, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, ReadOnlyMultimapConcept<
                TVertexMultimap, TVertex, TVertexCollection, TVertexEnumerator, TVertexEnumeratorProvider>>
            Create<TVertexMultimap, TVertexEnumeratorProvider>(
                TVertexMultimap neighborsByVertex, TVertexEnumeratorProvider vertexEnumeratorProvider)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, TVertexCollection>
            where TVertexEnumeratorProvider : IEnumeratorProvider<TVertexCollection, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));
            if (vertexEnumeratorProvider is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vertexEnumeratorProvider));

            ReadOnlyMultimapConcept<
                    TVertexMultimap, TVertex, TVertexCollection, TVertexEnumerator, TVertexEnumeratorProvider>
                vertexMultimapConcept = new(vertexEnumeratorProvider);
            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }
}
