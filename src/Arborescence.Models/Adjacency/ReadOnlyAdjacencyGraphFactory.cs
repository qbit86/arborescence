namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>
    /// objects.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>.
        /// </summary>
        /// <param name="neighborsByVertex">The object that provides the mapping from a vertex to its out-neighbors.</param>
        /// <param name="vertexMultimapConcept">The object that provides operations on the vertex multimap.</param>
        /// <typeparam name="TVertexMultimap">The type of mapping from a vertex to a sequence of its out-neighbors.</typeparam>
        /// <typeparam name="TVertexMultimapConcept">The type that provides operations on the vertex multimap.</typeparam>
        /// <returns>The read-only adjacency graph.</returns>
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept>
            Create<TVertexMultimap, TVertexMultimapConcept>(
                TVertexMultimap neighborsByVertex, TVertexMultimapConcept vertexMultimapConcept)
            where TVertexMultimapConcept : IReadOnlyMultimapConcept<TVertexMultimap, TVertex, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));
            if (vertexMultimapConcept is null)
                ArgumentNullExceptionHelpers.Throw(nameof(vertexMultimapConcept));

            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>
    /// objects, when <c>TVertexMultimapConcept</c> is
    /// <see cref="ReadOnlyMultimapConcept{TVertexMultimap, TVertex, TVertexCollection, TVertexEnumerator, TVertexEnumeratorProvider}"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TVertexCollection">The type of the vertex collection.</typeparam>
    /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TVertexCollection, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept}"/>,
        /// when <c>TVertexMultimapConcept</c> is
        /// <see cref="ReadOnlyMultimapConcept{TVertexMultimap, TVertex, TVertexCollection, TVertexEnumerator, TVertexEnumeratorProvider}"/>.
        /// </summary>
        /// <param name="neighborsByVertex">The object that provides the mapping from a vertex to its out-neighbors.</param>
        /// <param name="vertexEnumeratorProvider">The vertex enumerator provider.</param>
        /// <typeparam name="TVertexMultimap">The type of mapping from a vertex to a sequence of its out-neighbors.</typeparam>
        /// <typeparam name="TVertexEnumeratorProvider">The type of the vertex enumerator provider.</typeparam>
        /// <returns>The read-only adjacency graph.</returns>
        public static ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, ReadOnlyMultimapConcept<
                TVertexMultimap, TVertex, TVertexCollection, TVertexEnumerator, TVertexEnumeratorProvider>>
            Create<TVertexMultimap, TVertexEnumeratorProvider>(
                TVertexMultimap neighborsByVertex, TVertexEnumeratorProvider vertexEnumeratorProvider)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, TVertexCollection>
            where TVertexEnumeratorProvider : IEnumeratorProvider<TVertexCollection, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));
            if (vertexEnumeratorProvider is null)
                ArgumentNullExceptionHelpers.Throw(nameof(vertexEnumeratorProvider));

            ReadOnlyMultimapConcept<
                    TVertexMultimap, TVertex, TVertexCollection, TVertexEnumerator, TVertexEnumeratorProvider>
                vertexMultimapConcept = new(vertexEnumeratorProvider);
            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }
}
