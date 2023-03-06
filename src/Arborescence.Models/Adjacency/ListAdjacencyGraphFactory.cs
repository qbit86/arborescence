namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class AdjacencyGraphFactory<TVertex>
        where TVertex : notnull
    {
        public static ListAdjacencyGraph<TVertex, Dictionary<TVertex, List<TVertex>>> Create()
        {
            Dictionary<TVertex, List<TVertex>> neighborsByVertex = new();
            return new(neighborsByVertex);
        }

        public static ListAdjacencyGraph<TVertex, Dictionary<TVertex, List<TVertex>>> Create(
            IEqualityComparer<TVertex>? vertexComparer)
        {
            Dictionary<TVertex, List<TVertex>> neighborsByVertex = new(vertexComparer);
            return new(neighborsByVertex);
        }

        public static ListAdjacencyGraph<TVertex, TVertexMultimap> Create<TVertexMultimap>(
            TVertexMultimap neighborsByVertex)
            where TVertexMultimap : IDictionary<TVertex, List<TVertex>>, IReadOnlyDictionary<TVertex, List<TVertex>>
        {
            if (neighborsByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(neighborsByVertex));

            return new(neighborsByVertex);
        }
    }
}
