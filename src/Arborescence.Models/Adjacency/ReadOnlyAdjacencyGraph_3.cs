namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>
    /// objects, when <c>TVertexMultimapConcept</c> is
    /// <see cref="ReadOnlyMultimapConcept{TVertexMultimap,TVertex,TNeighborCollection,TNeighborEnumerator,TNeighborEnumeratorProvider}"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborCollection">The type of the neighbor collection.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static class ReadOnlyAdjacencyGraph<TVertex, TNeighborCollection, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>,
        /// when <c>TVertexMultimapConcept</c> is
        /// <see cref="ReadOnlyMultimapConcept{TVertexMultimap,TVertex,TVertexCollection,TNeighborEnumerator,TNeighborEnumeratorProvider}"/>.
        /// </summary>
        /// <param name="neighborsByVertex">The object that provides the mapping from a vertex to its out-neighbors.</param>
        /// <param name="neighborEnumeratorProvider">The neighbor enumerator provider.</param>
        /// <typeparam name="TVertexMultimap">The type of mapping from a vertex to a sequence of its out-neighbors.</typeparam>
        /// <typeparam name="TNeighborEnumeratorProvider">The type of the neighbor enumerator provider.</typeparam>
        /// <returns>The read-only adjacency graph.</returns>
        public static ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, ReadOnlyMultimapConcept<
                TVertexMultimap, TVertex, TNeighborCollection, TNeighborEnumerator, TNeighborEnumeratorProvider>>
            Create<TVertexMultimap, TNeighborEnumeratorProvider>(
                TVertexMultimap neighborsByVertex, TNeighborEnumeratorProvider neighborEnumeratorProvider)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, TNeighborCollection>
            where TNeighborEnumeratorProvider : IEnumeratorProvider<TNeighborCollection, TNeighborEnumerator>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));
            if (neighborEnumeratorProvider is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborEnumeratorProvider));

            ReadOnlyMultimapConcept<
                    TVertexMultimap, TVertex, TNeighborCollection, TNeighborEnumerator, TNeighborEnumeratorProvider>
                vertexMultimapConcept = new(neighborEnumeratorProvider);
            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }
}
