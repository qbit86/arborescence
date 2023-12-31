#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    public static partial class ReadOnlyAdjacencyGraphFactory<TVertex>
    {
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
    }
}
#endif
