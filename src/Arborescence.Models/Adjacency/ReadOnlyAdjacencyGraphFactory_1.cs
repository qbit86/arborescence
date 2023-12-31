namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static partial class ReadOnlyAdjacencyGraphFactory<TVertex>
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
