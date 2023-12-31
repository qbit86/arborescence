#if NET8_0_OR_GREATER
namespace Arborescence.Models
{
    using System.Collections.Frozen;
    using System.Collections.Generic;

    public static partial class ReadOnlyAdjacencyGraphFactory<TVertex>
    {
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
