#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    public static partial class ReadOnlyIncidenceGraphFactory<TVertex, TEdge>
    {
        public static ReadOnlyIncidenceGraph<
                TVertex,
                TEdge,
                ArraySegment<TEdge>.Enumerator,
                TEndpointMap,
                TEdgeMultimap,
                ReadOnlyMultimapConcept<
                    TEdgeMultimap,
                    TVertex,
                    TEdge[],
                    ArraySegment<TEdge>.Enumerator,
                    ArrayEnumeratorProvider<TEdge>>>
            FromArrays<TEndpointMap, TEdgeMultimap>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IReadOnlyDictionary<TVertex, TEdge[]>
        {
            if (tailByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(tailByEdge));
            if (headByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(outEdgesByVertex));

            ReadOnlyMultimapConcept<
                    TEdgeMultimap,
                    TVertex,
                    TEdge[],
                    ArraySegment<TEdge>.Enumerator,
                    ArrayEnumeratorProvider<TEdge>>
                edgeMultimapConcept = new(default);
            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapConcept);
        }

        public static ReadOnlyIncidenceGraph<
                TVertex,
                TEdge,
                ArraySegment<TEdge>.Enumerator,
                TEndpointMap,
                TEdgeMultimap,
                ReadOnlyMultimapConcept<
                    TEdgeMultimap,
                    TVertex,
                    ArraySegment<TEdge>,
                    ArraySegment<TEdge>.Enumerator,
                    ArraySegmentEnumeratorProvider<TEdge>>>
            FromArraySegments<TEndpointMap, TEdgeMultimap>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IReadOnlyDictionary<TVertex, ArraySegment<TEdge>>
        {
            if (tailByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(tailByEdge));
            if (headByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(outEdgesByVertex));

            ReadOnlyMultimapConcept<
                    TEdgeMultimap,
                    TVertex,
                    ArraySegment<TEdge>,
                    ArraySegment<TEdge>.Enumerator,
                    ArraySegmentEnumeratorProvider<TEdge>>
                edgeMultimapConcept = new(default);
            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapConcept);
        }
    }
}
#endif
