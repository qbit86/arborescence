#if NET8_0_OR_GREATER
namespace Arborescence.Models
{
    using System.Collections.Frozen;
    using System.Collections.Generic;

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
                FrozenSet<TVertex>.Enumerator,
                TVertexMultimap,
                ReadOnlyMultimapConcept<
                    TVertexMultimap,
                    TVertex,
                    FrozenSet<TVertex>,
                    FrozenSet<TVertex>.Enumerator,
                    FrozenSetEnumeratorProvider<TVertex>>>
            FromFrozenSets<TVertexMultimap>(TVertexMultimap neighborsByVertex)
            where TVertexMultimap : IReadOnlyDictionary<TVertex, FrozenSet<TVertex>>
        {
            if (neighborsByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(neighborsByVertex));

            ReadOnlyMultimapConcept<
                    TVertexMultimap,
                    TVertex,
                    FrozenSet<TVertex>,
                    FrozenSet<TVertex>.Enumerator,
                    FrozenSetEnumeratorProvider<TVertex>>
                vertexMultimapConcept = new(default);
            return new(neighborsByVertex, vertexMultimapConcept);
        }
    }
}
#endif
