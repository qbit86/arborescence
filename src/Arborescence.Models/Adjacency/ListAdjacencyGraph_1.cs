namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/> type.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    public static class ListAdjacencyGraph<TVertex>
        where TVertex : notnull
    {
        /// <summary>
        /// Creates an empty <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// where <c>TVertexMultimap</c> is <see cref="Dictionary{TKey, TValue}"/>
        /// that maps from a vertex to its out-neighbors.
        /// </summary>
        /// <returns>An empty <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>.</returns>
        public static ListAdjacencyGraph<TVertex, Dictionary<TVertex, List<TVertex>>> Create()
        {
            Dictionary<TVertex, List<TVertex>> neighborsByVertex = new();
            return new(neighborsByVertex);
        }

        /// <summary>
        /// Creates an empty <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// with the specified comparer,
        /// where <c>TVertexMultimap</c> is <see cref="Dictionary{TKey, TValue}"/>
        /// that maps from a vertex to its out-neighbors.
        /// </summary>
        /// <param name="vertexComparer">The comparer implementation to use to compare vertices for equality.</param>
        /// <returns>
        /// An empty <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/> with the specified comparer.
        /// </returns>
        public static ListAdjacencyGraph<TVertex, Dictionary<TVertex, List<TVertex>>> Create(
            IEqualityComparer<TVertex>? vertexComparer)
        {
            Dictionary<TVertex, List<TVertex>> neighborsByVertex = new(vertexComparer);
            return new(neighborsByVertex);
        }

        /// <summary>
        /// Creates a <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// with the specified underlying multimap.
        /// </summary>
        /// <param name="neighborsByVertex">The object that provides the mapping from a vertex to its out-neighbors.</param>
        /// <typeparam name="TVertexMultimap">The type of mapping from a vertex to a sequence of its out-neighbors.</typeparam>
        /// <returns>
        /// A <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/> containing vertices and edges
        /// defined by <paramref name="neighborsByVertex"/>.
        /// </returns>
        public static ListAdjacencyGraph<TVertex, TVertexMultimap> Create<TVertexMultimap>(
            TVertexMultimap neighborsByVertex)
            where TVertexMultimap : IDictionary<TVertex, List<TVertex>>, IReadOnlyDictionary<TVertex, List<TVertex>>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));

            return new(neighborsByVertex);
        }
    }
}
