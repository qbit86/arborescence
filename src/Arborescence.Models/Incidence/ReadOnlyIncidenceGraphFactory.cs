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

    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge, TEdgeCollection, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static ReadOnlyIncidenceGraph<TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap,
                ReadOnlyMultimapPolicy<TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeMultimap, TEdgeCollectionPolicy>>
            CreateUnchecked<TEndpointMap, TEdgeMultimap, TEdgeCollectionPolicy>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
                TEdgeCollectionPolicy edgeCollectionPolicy)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IReadOnlyDictionary<TVertex, TEdgeCollection>
            where TEdgeCollectionPolicy : IEnumerablePolicy<TEdgeCollection, TEdgeEnumerator>
        {
            if (tailByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(tailByEdge));
            if (headByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));
            if (edgeCollectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edgeCollectionPolicy));

            ReadOnlyMultimapPolicy<TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeMultimap, TEdgeCollectionPolicy>
                edgeMultimapPolicy = new(edgeCollectionPolicy);
            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapPolicy);
        }
    }
}
