namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    /// <summary>
    /// Implements an adjacency graph as a dictionary that maps vertices of type <typeparamref name="TVertex"/>
    /// to lists of out-neighbors of type <see cref="List{TVertex}"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TVertexMultimap">
    /// The type of dictionary that maps from a vertex to a list of its out-neighbors of type <see cref="List{TVertex}"/>.
    /// </typeparam>
    public readonly partial struct ListAdjacencyGraph<TVertex, TVertexMultimap> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, List<TVertex>.Enumerator>>,
        IOutNeighborsAdjacency<TVertex, List<TVertex>.Enumerator>,
        IEquatable<ListAdjacencyGraph<TVertex, TVertexMultimap>>
        where TVertexMultimap : IDictionary<TVertex, List<TVertex>>, IReadOnlyDictionary<TVertex, List<TVertex>>
    {
        private static ListMultimapConcept<TVertexMultimap, TVertex> MultimapConcept => default;

        private readonly TVertexMultimap _neighborsByVertex;

        internal ListAdjacencyGraph(TVertexMultimap neighborsByVertex) => _neighborsByVertex = neighborsByVertex;

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount
        {
            get
            {
                TVertexMultimap? neighborsByVertex = _neighborsByVertex;
                return neighborsByVertex is null ? 0 : GetCountUnchecked(neighborsByVertex);
            }
        }

        /// <inheritdoc/>
        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        /// <inheritdoc/>
        public IncidenceEnumerator<TVertex, List<TVertex>.Enumerator> EnumerateOutEdges(TVertex vertex)
        {
            List<TVertex>.Enumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return new(vertex, neighborEnumerator);
        }

        /// <inheritdoc/>
        public List<TVertex>.Enumerator EnumerateOutNeighbors(TVertex vertex) =>
            MultimapConcept.EnumerateValues(_neighborsByVertex, vertex);

        /// <summary>
        /// Adds the edge to the graph with the specified tail and head.
        /// </summary>
        /// <param name="tail">The tail of the edge to add.</param>
        /// <param name="head">The head of the edge to add.</param>
        public void AddEdge(TVertex tail, TVertex head)
        {
            TVertexMultimap neighborsByVertex = _neighborsByVertex;
            if (TryGetValue(neighborsByVertex, tail, out List<TVertex>? neighbors))
            {
                neighbors.Add(head);
            }
            else
            {
                neighbors = new(1) { head };
                neighborsByVertex.Add(tail, neighbors);
            }

            if (!ContainsKey(neighborsByVertex, head))
                neighborsByVertex.Add(head, new());
        }

        /// <summary>
        /// Attempts to add the vertex to the graph.
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        /// <returns>
        /// <see langword="false"/> if the <see cref="ListAdjacencyGraph{TVertex, TVertexMultimap}"/>
        /// already contains the specified vertex;
        /// otherwise, <see langword="true"/>.
        /// </returns>
        public bool TryAddVertex(TVertex vertex)
        {
            TVertexMultimap neighborsByVertex = _neighborsByVertex;
            if (ContainsKey(neighborsByVertex, vertex))
                return false;
            neighborsByVertex.Add(vertex, new());
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out List<TVertex>? value)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TVertex>> =>
            dictionary.TryGetValue(vertex, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ContainsKey<TDictionary>(TDictionary dictionary, TVertex vertex)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TVertex>> =>
            dictionary.ContainsKey(vertex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetCountUnchecked<TDictionary>(TDictionary dictionary)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TVertex>> => dictionary.Count;
    }
}
