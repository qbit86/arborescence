namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>
    /// objects.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    public static partial class ReadOnlyAdjacencyGraphFactory<TVertex>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>.
        /// </summary>
        /// <param name="neighborsByVertex">The object that provides the mapping from a vertex to its out-neighbors.</param>
        /// <typeparam name="TVertexMultimap">The type of mapping from a vertex to a sequence of its out-neighbors.</typeparam>
        /// <returns>The read-only adjacency graph.</returns>
        public static ReadOnlyAdjacencyGraph<
                TVertex,
                List<TVertex>.Enumerator,
                TVertexMultimap,
                ReadOnlyMultimapConcept<
                    TVertexMultimap,
                    TVertex,
                    List<TVertex>,
                    List<TVertex>.Enumerator,
                    ListEnumeratorProvider<TVertex>>>
            FromLists<TVertexMultimap>(TVertexMultimap neighborsByVertex)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, List<TVertex>>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));

            ReadOnlyMultimapConcept<
                    TVertexMultimap, TVertex, List<TVertex>, List<TVertex>.Enumerator, ListEnumeratorProvider<TVertex>>
                vertexMultimapConcept = new(default);
            return new(neighborsByVertex, vertexMultimapConcept);
        }

        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>.
        /// </summary>
        /// <param name="neighborsByVertex">The object that provides the mapping from a vertex to its out-neighbors.</param>
        /// <typeparam name="TVertexMultimap">The type of mapping from a vertex to a sequence of its out-neighbors.</typeparam>
        /// <returns>The read-only adjacency graph.</returns>
        public static ReadOnlyAdjacencyGraph<
                TVertex,
                HashSet<TVertex>.Enumerator,
                TVertexMultimap,
                ReadOnlyMultimapConcept<
                    TVertexMultimap,
                    TVertex,
                    HashSet<TVertex>,
                    HashSet<TVertex>.Enumerator,
                    HashSetEnumeratorProvider<TVertex>>>
            FromHashSets<TVertexMultimap>(TVertexMultimap neighborsByVertex)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, HashSet<TVertex>>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));

            ReadOnlyMultimapConcept<
                    TVertexMultimap,
                    TVertex,
                    HashSet<TVertex>,
                    HashSet<TVertex>.Enumerator,
                    HashSetEnumeratorProvider<TVertex>>
                vertexMultimapConcept = new(default);
            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }
}
