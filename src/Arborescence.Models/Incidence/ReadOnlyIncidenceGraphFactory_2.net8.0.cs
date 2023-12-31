#if NET8_0_OR_GREATER
namespace Arborescence.Models
{
    using System.Collections.Frozen;
    using System.Collections.Generic;

    public static partial class ReadOnlyIncidenceGraphFactory<TVertex, TEdge>
    {
        public static ReadOnlyIncidenceGraph<
                TVertex,
                TEdge,
                FrozenSet<TEdge>.Enumerator,
                TEndpointMap,
                TEdgeMultimap,
                ReadOnlyMultimapConcept<
                    TEdgeMultimap,
                    TVertex,
                    FrozenSet<TEdge>,
                    FrozenSet<TEdge>.Enumerator,
                    FrozenSetEnumeratorProvider<TEdge>>>
            FromFrozenSets<TEndpointMap, TEdgeMultimap>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IReadOnlyDictionary<TVertex, FrozenSet<TEdge>>
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
                    FrozenSet<TEdge>,
                    FrozenSet<TEdge>.Enumerator,
                    FrozenSetEnumeratorProvider<TEdge>>
                edgeMultimapConcept = new(default);
            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapConcept);
        }
    }
}
#endif
