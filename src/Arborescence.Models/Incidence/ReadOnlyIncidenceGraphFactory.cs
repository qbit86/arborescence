namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge>
    {
        public static ReadOnlyIncidenceGraph<
                TVertex,
                TEdge,
                List<TEdge>.Enumerator,
                TEndpointMap,
                TEdgeMultimap,
                ReadOnlyMultimapConcept<
                    TEdgeMultimap,
                    TVertex,
                    List<TEdge>,
                    List<TEdge>.Enumerator,
                    ListEnumeratorProvider<TEdge>>>
            FromLists<TEndpointMap, TEdgeMultimap>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IReadOnlyDictionary<TVertex, List<TEdge>>
        {
            if (tailByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(tailByEdge));
            if (headByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(outEdgesByVertex));

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyIncidenceGraph{TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept}"/>
    /// objects.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyIncidenceGraph{TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept}"/>.
        /// </summary>
        /// <param name="tailByEdge">The object that provides the mapping from an edge to its tail.</param>
        /// <param name="headByEdge">The object that provides the mapping from an edge to its head.</param>
        /// <param name="outEdgesByVertex">The object that provides the mapping from a vertex to its out-edges.</param>
        /// <param name="edgeMultimapConcept">The object that provides operations on the edge multimap.</param>
        /// <typeparam name="TEndpointMap">The type of mapping from an edge to one of its endpoints.</typeparam>
        /// <typeparam name="TEdgeMultimap">The type of mapping from a vertex to a sequence of its out-edges.</typeparam>
        /// <typeparam name="TEdgeMultimapConcept">The type that provides operations on the edge multimap.</typeparam>
        /// <remarks>
        /// <paramref name="tailByEdge"/> and <paramref name="outEdgesByVertex"/>
        /// should be consistent in the sense that<br/>
        /// ∀v ∀e   e ∈ out-edges(v) ⇔ v = tail(e)<br/>
        /// but this property is not checked.
        /// </remarks>
        /// <returns>The read-only incidence graph.</returns>
        public static ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>
            CreateUnchecked<TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
                TEdgeMultimapConcept edgeMultimapConcept)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimapConcept : IReadOnlyMultimapConcept<TEdgeMultimap, TVertex, TEdgeEnumerator>
        {
            if (tailByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(tailByEdge));
            if (headByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(outEdgesByVertex));
            if (edgeMultimapConcept is null)
                ArgumentNullExceptionHelpers.Throw(nameof(edgeMultimapConcept));

            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapConcept);
        }
    }

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyIncidenceGraph{TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept}"/>
    /// objects, when <c>TEdgeMultimapConcept</c> is
    /// <see cref="ReadOnlyMultimapConcept{TEdgeMultimap, TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeEnumeratorProvider}"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeCollection">The type of the edge collection.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static class ReadOnlyIncidenceGraphFactory<TVertex, TEdge, TEdgeCollection, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyIncidenceGraph{TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept}"/>,
        /// when <c>TEdgeMultimapConcept</c> is
        /// <see cref="ReadOnlyMultimapConcept{TEdgeMultimap, TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeEnumeratorProvider}"/>.
        /// </summary>
        /// <param name="tailByEdge">The object that provides the mapping from an edge to its tail.</param>
        /// <param name="headByEdge">The object that provides the mapping from an edge to its head.</param>
        /// <param name="outEdgesByVertex">The object that provides the mapping from a vertex to its out-edges.</param>
        /// <param name="edgeEnumeratorProvider">The edge enumerator provider.</param>
        /// <typeparam name="TEndpointMap">The type of mapping from an edge to one of its endpoints.</typeparam>
        /// <typeparam name="TEdgeMultimap">The type of mapping from a vertex to a sequence of its out-edges.</typeparam>
        /// <typeparam name="TEdgeEnumeratorProvider">The type of the edge enumerator provider.</typeparam>
        /// <remarks>
        /// <paramref name="tailByEdge"/> and <paramref name="outEdgesByVertex"/>
        /// should be consistent in the sense that<br/>
        /// ∀v ∀e   e ∈ out-edges(v) ⇔ v = tail(e)<br/>
        /// but this property is not checked.
        /// </remarks>
        /// <returns>The read-only incidence graph.</returns>
        public static ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, ReadOnlyMultimapConcept<
                    TEdgeMultimap, TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeEnumeratorProvider>>
            CreateUnchecked<TEndpointMap, TEdgeMultimap, TEdgeEnumeratorProvider>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
                TEdgeEnumeratorProvider edgeEnumeratorProvider)
            where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IReadOnlyDictionary<TVertex, TEdgeCollection>
            where TEdgeEnumeratorProvider : IEnumeratorProvider<TEdgeCollection, TEdgeEnumerator>
        {
            if (tailByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(tailByEdge));
            if (headByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(outEdgesByVertex));
            if (edgeEnumeratorProvider is null)
                ArgumentNullExceptionHelpers.Throw(nameof(edgeEnumeratorProvider));

            ReadOnlyMultimapConcept<TEdgeMultimap, TVertex, TEdgeCollection, TEdgeEnumerator, TEdgeEnumeratorProvider>
                edgeMultimapConcept = new(edgeEnumeratorProvider);
            return new(tailByEdge, headByEdge, outEdgesByVertex, edgeMultimapConcept);
        }
    }
}
