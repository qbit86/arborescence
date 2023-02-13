namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge, TEdgeCollection, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEndpointMap, TEdgesMap, TEdgeCollection, TEdgeEnumerator, TEdgeCollectionPolicy>
            CreateUnchecked<TEndpointMap, TEdgesMap, TEdgeCollectionPolicy>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgesMap outEdgesByVertex,
                TEdgeCollectionPolicy edgeCollectionPolicy)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgesMap : IReadOnlyDictionary<TVertex, TEdgeCollection>
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

            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeCollectionPolicy);
        }
    }
}
