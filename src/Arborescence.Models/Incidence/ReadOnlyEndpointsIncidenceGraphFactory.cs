namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;

    public static class ReadOnlyEndpointsIncidenceGraphFactory<TVertex, TEdgeCollection, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<Endpoints<TVertex>>
    {
        public static ReadOnlyEndpointsIncidenceGraph<
                TVertex, TEdgesMap, TEdgeCollection, TEdgeEnumerator, TEdgeCollectionPolicy>
            CreateUnchecked<TEdgesMap, TEdgeCollectionPolicy>(
                TEdgesMap outEdgesByVertex, TEdgeCollectionPolicy edgeCollectionPolicy)
            where TEdgesMap : IReadOnlyDictionary<TVertex, TEdgeCollection>
            where TEdgeCollectionPolicy : IEnumerablePolicy<TEdgeCollection, TEdgeEnumerator>
        {
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));
            if (edgeCollectionPolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edgeCollectionPolicy));

            return new(outEdgesByVertex, edgeCollectionPolicy);
        }
    }
}
