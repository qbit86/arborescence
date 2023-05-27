namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>
            CreateUnchecked<TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
                TEdgeMultimapConcept edgeMultimapConcept)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimapConcept : IReadOnlyMultimapConcept<TVertex, TEdgeMultimap, TEdgeEnumerator>
        {
            if (tailByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(tailByEdge));
            if (headByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));
            if (edgeMultimapConcept is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edgeMultimapConcept));

            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapConcept);
        }
    }

    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge, TEdgeCollection, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, ReadOnlyMultimapConcept<
                    TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeMultimap, TEdgeEnumeratorProvider>>
            CreateUnchecked<TEndpointMap, TEdgeMultimap, TEdgeEnumeratorProvider>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
                TEdgeEnumeratorProvider edgeEnumeratorProvider)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IReadOnlyDictionary<TVertex, TEdgeCollection>
            where TEdgeEnumeratorProvider : IEnumeratorProvider<TEdgeCollection, TEdgeEnumerator>
        {
            if (tailByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(tailByEdge));
            if (headByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));
            if (edgeEnumeratorProvider is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edgeEnumeratorProvider));

            ReadOnlyMultimapConcept<TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeMultimap, TEdgeEnumeratorProvider>
                edgeMultimapConcept = new(edgeEnumeratorProvider);
            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapConcept);
        }
    }
}
