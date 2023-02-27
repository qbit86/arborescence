namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>
            CreateUnchecked<TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
                TEdgeMultimapPolicy edgeMultimapPolicy)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimapPolicy : IReadOnlyMultimapPolicy<TVertex, TEdgeMultimap, TEdgeEnumerator>
        {
            if (tailByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(tailByEdge));
            if (headByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));
            if (edgeMultimapPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edgeMultimapPolicy));

            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapPolicy);
        }
    }
}
