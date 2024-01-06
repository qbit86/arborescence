namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    /// <summary>
    /// Represents a read-only adjacency graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    /// <typeparam name="TVertexMultimap">The type of dictionary that maps from a vertex to a sequence of its out-neighbors.</typeparam>
    /// <typeparam name="TVertexMultimapConcept">The type that provides operations on the vertex multimap.</typeparam>
    public readonly partial struct ReadOnlyAdjacencyGraph<
        TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, TNeighborEnumerator>>,
        IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>,
        IEquatable<ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept>>
        where TNeighborEnumerator : IEnumerator<TVertex>
        where TVertexMultimapConcept : IReadOnlyMultimapConcept<TVertexMultimap, TVertex, TNeighborEnumerator>
    {
        private readonly TVertexMultimap _neighborsByVertex;
        private readonly TVertexMultimapConcept _vertexMultimapConcept;

        internal ReadOnlyAdjacencyGraph(TVertexMultimap neighborsByVertex, TVertexMultimapConcept vertexMultimapConcept)
        {
            _neighborsByVertex = neighborsByVertex;
            _vertexMultimapConcept = vertexMultimapConcept;
        }

        /// <summary>
        /// Gets the number of vertices that can have out-neighbors.
        /// </summary>
        public int TailCount
        {
            get
            {
                ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> self =
                    this;
                return self._neighborsByVertex is null ? 0 : self.GetCountUnchecked();
            }
        }

        /// <inheritdoc/>
        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        /// <inheritdoc/>
        public IncidenceEnumerator<TVertex, TNeighborEnumerator> EnumerateOutEdges(TVertex vertex)
        {
            TNeighborEnumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return IncidenceEnumerator.Create(vertex, neighborEnumerator);
        }

        /// <inheritdoc/>
        public TNeighborEnumerator EnumerateOutNeighbors(TVertex vertex)
        {
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return self._vertexMultimapConcept.EnumerateValues(self._neighborsByVertex, vertex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetCountUnchecked()
        {
            ReadOnlyAdjacencyGraph<TVertex, TNeighborEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return self._vertexMultimapConcept.GetCount(self._neighborsByVertex);
        }
    }
}
