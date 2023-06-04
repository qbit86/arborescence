namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/> type.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public static class ListIncidenceGraphFactory<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : notnull
    {
        /// <summary>
        /// Creates an empty <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>
        /// where <c>TEndpointMap</c> is <see cref="Dictionary{TKey, TValue}"/>
        /// that maps from an edge to one of its endpoints,
        /// and <c>TEdgeMultimap</c> is <see cref="Dictionary{TKey, TValue}"/>
        /// that maps from a vertex to its out-edges.
        /// </summary>
        /// <returns>
        /// An empty <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>.
        /// </returns>
        public static ListIncidenceGraph<TVertex, TEdge, Dictionary<TEdge, TVertex>, Dictionary<TVertex, List<TEdge>>>
            Create()
        {
            Dictionary<TEdge, TVertex> tailByEdge = new();
            Dictionary<TEdge, TVertex> headByEdge = new();
            Dictionary<TVertex, List<TEdge>> outEdgesByVertex = new();
            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }

        /// <summary>
        /// Creates an empty <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>
        /// with the specified comparers,
        /// where <c>TEndpointMap</c> is <see cref="Dictionary{TKey, TValue}"/>
        /// that maps from an edge to one of its endpoints,
        /// and <c>TEdgeMultimap</c> is <see cref="Dictionary{TKey, TValue}"/>
        /// that maps from a vertex to its out-edges.
        /// </summary>
        /// <param name="vertexComparer">The comparer implementation to use to compare vertices for equality.</param>
        /// <param name="edgeComparer">The comparer implementation to use to compare edges for equality.</param>
        /// <returns>
        /// An empty <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>.
        /// </returns>
        public static ListIncidenceGraph<TVertex, TEdge, Dictionary<TEdge, TVertex>, Dictionary<TVertex, List<TEdge>>>
            Create(IEqualityComparer<TVertex>? vertexComparer, IEqualityComparer<TEdge>? edgeComparer)
        {
            Dictionary<TEdge, TVertex> tailByEdge = new(edgeComparer);
            Dictionary<TEdge, TVertex> headByEdge = new(edgeComparer);
            Dictionary<TVertex, List<TEdge>> outEdgesByVertex = new(vertexComparer);
            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }

        /// <summary>
        /// Creates a <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>.
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
        /// <returns>
        /// A <see cref="ListIncidenceGraph{TVertex, TEdge, TEndpointMap, TEdgeMultimap}"/>
        /// containing vertices and edges defined by <paramref name="tailByEdge"/>, <paramref name="headByEdge"/>,
        /// and <paramref name="outEdgesByVertex"/>.
        /// </returns>
        public static ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>
            CreateUnchecked<TEndpointMap, TEdgeMultimap>(
                TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex)
            where TEndpointMap : IDictionary<TEdge, TVertex>, IReadOnlyDictionary<TEdge, TVertex>
            where TEdgeMultimap : IDictionary<TVertex, List<TEdge>>, IReadOnlyDictionary<TVertex, List<TEdge>>
        {
            if (tailByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(tailByEdge));
            if (headByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));

            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }
    }
}
