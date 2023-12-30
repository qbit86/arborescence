namespace Arborescence.Models
{
    using System.Collections.Generic;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    using System;
#endif

    public static class ReadOnlyAdjacencyGraphFactory<TVertex>
    {
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

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        public static ReadOnlyAdjacencyGraph<
                TVertex,
                ArraySegment<TVertex>.Enumerator,
                TVertexMultimap,
                ReadOnlyMultimapConcept<
                    TVertexMultimap,
                    TVertex,
                    TVertex[],
                    ArraySegment<TVertex>.Enumerator,
                    ArrayEnumeratorProvider<TVertex>>>
            FromArrays<TVertexMultimap>(TVertexMultimap neighborsByVertex)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, TVertex[]>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));

            ReadOnlyMultimapConcept<
                    TVertexMultimap, TVertex, TVertex[], ArraySegment<TVertex>.Enumerator,
                    ArrayEnumeratorProvider<TVertex>>
                vertexMultimapConcept = new(default);
            return new(neighborsByVertex, vertexMultimapConcept);
        }

        public static ReadOnlyAdjacencyGraph<
                TVertex,
                ArraySegment<TVertex>.Enumerator,
                TVertexMultimap,
                ReadOnlyMultimapConcept<
                    TVertexMultimap,
                    TVertex,
                    ArraySegment<TVertex>,
                    ArraySegment<TVertex>.Enumerator,
                    ArraySegmentEnumeratorProvider<TVertex>>>
            FromArraySegments<TVertexMultimap>(TVertexMultimap neighborsByVertex)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, ArraySegment<TVertex>>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));

            ReadOnlyMultimapConcept<
                    TVertexMultimap, TVertex, ArraySegment<TVertex>, ArraySegment<TVertex>.Enumerator,
                    ArraySegmentEnumeratorProvider<TVertex>>
                vertexMultimapConcept = new(default);
            return new(neighborsByVertex, vertexMultimapConcept);
        }
#endif
    }

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>
    /// objects.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>.
        /// </summary>
        /// <param name="neighborsByVertex">The object that provides the mapping from a vertex to its out-neighbors.</param>
        /// <param name="vertexMultimapConcept">The object that provides operations on the vertex multimap.</param>
        /// <typeparam name="TVertexMultimap">The type of mapping from a vertex to a sequence of its out-neighbors.</typeparam>
        /// <typeparam name="TVertexMultimapConcept">The type that provides operations on the vertex multimap.</typeparam>
        /// <returns>The read-only adjacency graph.</returns>
        public static ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept>
            Create<TVertexMultimap, TVertexMultimapConcept>(
                TVertexMultimap neighborsByVertex, TVertexMultimapConcept vertexMultimapConcept)
            where TVertexMultimapConcept : IReadOnlyMultimapConcept<TVertexMultimap, TVertex, TNeighborEnumerator>
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
    /// <see cref="ReadOnlyAdjacencyGraph{TVertex,TNeighborEnumerator,TVertexMultimap,TVertexMultimapConcept}"/>
    /// objects, when <c>TVertexMultimapConcept</c> is
    /// <see cref="ReadOnlyMultimapConcept{TVertexMultimap,TVertex,TNeighborCollection,TNeighborEnumerator,TNeighborEnumeratorProvider}"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborCollection">The type of the neighbor collection.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static class ReadOnlyAdjacencyGraphFactory<TVertex, TNeighborCollection, TNeighborEnumerator>
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
