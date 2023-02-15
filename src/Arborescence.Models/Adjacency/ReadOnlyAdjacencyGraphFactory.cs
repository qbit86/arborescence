namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy>
            Create<TVertexMultimap, TVertexMultimapPolicy>(
                TVertexMultimap neighborsByVertex, TVertexMultimapPolicy vertexMultimapPolicy)
            where TVertexMultimapPolicy : IMultimapPolicy<TVertex, TVertexMultimap, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));
            if (vertexMultimapPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(vertexMultimapPolicy));

            return new(neighborsByVertex, vertexMultimapPolicy);
        }
    }
}
