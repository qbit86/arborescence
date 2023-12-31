#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;

    public static partial class ReadOnlyIncidenceGraphFactory<TVertex, TEdge>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyIncidenceGraph{TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept}"/>.
        /// </summary>
        /// <param name="tailByEdge">The object that provides the mapping from an edge to its tail.</param>
        /// <param name="headByEdge">The object that provides the mapping from an edge to its head.</param>
        /// <param name="outEdgesByVertex">The object that provides the mapping from a vertex to its out-edges.</param>
        /// <typeparam name="TEndpointMap">The type of mapping from an edge to one of its endpoints.</typeparam>
        /// <typeparam name="TEdgeMultimap">The type of mapping from a vertex to a sequence of its out-edges.</typeparam>
        /// <remarks>
        /// <paramref name="tailByEdge"/> and <paramref name="outEdgesByVertex"/>
        /// should be consistent in the sense that<br/>
        /// ∀v ∀e   e ∈ out-edges(v) ⇔ v = tail(e)<br/>
        /// but this property is not checked.
        /// </remarks>
        /// <returns>The read-only incidence graph.</returns>
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

        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyIncidenceGraph{TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept}"/>.
        /// </summary>
        /// <param name="tailByEdge">The object that provides the mapping from an edge to its tail.</param>
        /// <param name="headByEdge">The object that provides the mapping from an edge to its head.</param>
        /// <param name="outEdgesByVertex">The object that provides the mapping from a vertex to its out-edges.</param>
        /// <typeparam name="TEndpointMap">The type of mapping from an edge to one of its endpoints.</typeparam>
        /// <typeparam name="TEdgeMultimap">The type of mapping from a vertex to a sequence of its out-edges.</typeparam>
        /// <remarks>
        /// <paramref name="tailByEdge"/> and <paramref name="outEdgesByVertex"/>
        /// should be consistent in the sense that<br/>
        /// ∀v ∀e   e ∈ out-edges(v) ⇔ v = tail(e)<br/>
        /// but this property is not checked.
        /// </remarks>
        /// <returns>The read-only incidence graph.</returns>
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
