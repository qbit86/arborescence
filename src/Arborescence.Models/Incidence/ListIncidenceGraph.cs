namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Implements an incidence graph as a dictionary that maps vertices of type <see cref="TVertex"/>
    /// to lists of out-edges of type <see cref="List{TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEndpointMap">The type of mapping from an edge to one of its endpoints.</typeparam>
    /// <typeparam name="TEdgeMultimap">
    /// The type of dictionary that maps from a vertex to a list of its out-edges of type <see cref="List{TEdge}"/>.
    /// </typeparam>
    public readonly partial struct ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, List<TEdge>.Enumerator>,
        IOutNeighborsAdjacency<TVertex,
            AdjacencyEnumerator<TVertex, TEdge, ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>,
                List<TEdge>.Enumerator>>,
        IEquatable<ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>>
        where TEndpointMap : IDictionary<TEdge, TVertex>
        where TEdgeMultimap : IDictionary<TVertex, List<TEdge>>, IReadOnlyDictionary<TVertex, List<TEdge>>
    {
        private static ListMultimapConcept<TEdgeMultimap, TVertex, TEdge> MultimapConcept => default;

        private readonly TEndpointMap _tailByEdge;
        private readonly TEndpointMap _headByEdge;
        private readonly TEdgeMultimap _outEdgesByVertex;

        internal ListIncidenceGraph(TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex)
        {
            _tailByEdge = tailByEdge;
            _headByEdge = headByEdge;
            _outEdgesByVertex = outEdgesByVertex;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount
        {
            get
            {
                ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
                return self._outEdgesByVertex is null ? 0 : GetCountUnchecked(self._outEdgesByVertex);
            }
        }

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount
        {
            get
            {
                TEndpointMap? headByEdge = _headByEdge;
                return headByEdge is null ? 0 : headByEdge.Count;
            }
        }

        /// <inheritdoc/>
        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _tailByEdge.TryGetValue(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _headByEdge.TryGetValue(edge, out head);

        /// <inheritdoc/>
        public List<TEdge>.Enumerator EnumerateOutEdges(TVertex vertex) =>
            MultimapConcept.EnumerateValues(_outEdgesByVertex, vertex);

        /// <inheritdoc/>
        public AdjacencyEnumerator<
                TVertex, TEdge, ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>, List<TEdge>.Enumerator>
            EnumerateOutNeighbors(TVertex vertex)
        {
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            List<TEdge>.Enumerator edgeEnumerator = self.EnumerateOutEdges(vertex);
            return AdjacencyEnumeratorFactory<TVertex, TEdge>.Create(self, edgeEnumerator);
        }

        /// <summary>
        /// Adds the edge to the graph with the specified tail and head.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        /// <param name="tail">The tail of the edge to add.</param>
        /// <param name="head">The head of the edge to add.</param>
        /// <returns></returns>
        public bool TryAddEdge(TEdge edge, TVertex tail, TVertex head)
        {
            ListIncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            if (!self._tailByEdge.TryAddStrict(edge, tail))
                return false;

            if (TryGetValue(self._outEdgesByVertex, tail, out List<TEdge>? outEdges))
            {
                outEdges.Add(edge);
            }
            else
            {
                outEdges = new(1) { edge };
                self._outEdgesByVertex.Add(tail, outEdges);
            }

            if (!ContainsKey(self._outEdgesByVertex, head))
                self._outEdgesByVertex.Add(head, new());

            // ReSharper disable once PossibleStructMemberModificationOfNonVariableStruct
            self._headByEdge[edge] = head;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out List<TEdge>? value)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TEdge>> =>
            dictionary.TryGetValue(vertex, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsKey<TDictionary>(TDictionary dictionary, TVertex vertex)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TEdge>> =>
            dictionary.ContainsKey(vertex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetCountUnchecked<TDictionary>(TDictionary dictionary)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TEdge>> => dictionary.Count;
    }
}
