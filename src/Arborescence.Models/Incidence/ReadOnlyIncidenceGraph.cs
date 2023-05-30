namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public readonly partial struct ReadOnlyIncidenceGraph<
        TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>,
        IOutNeighborsAdjacency<TVertex, AdjacencyEnumerator<
            TVertex, TEdge,
            ReadOnlyIncidenceGraph<TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>,
            TEdgeEnumerator>>,
        IEquatable<ReadOnlyIncidenceGraph<
            TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>>
        where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TEdgeMultimapConcept : IReadOnlyMultimapConcept<TEdgeMultimap, TVertex, TEdgeEnumerator>
    {
        private readonly TEndpointMap _tailByEdge;
        private readonly TEndpointMap _headByEdge;
        private readonly TEdgeMultimap _outEdgesByVertex;
        private readonly TEdgeMultimapConcept _edgeMultimapConcept;

        internal ReadOnlyIncidenceGraph(
            TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
            TEdgeMultimapConcept edgeMultimapConcept)
        {
            _tailByEdge = tailByEdge;
            _headByEdge = headByEdge;
            _outEdgesByVertex = outEdgesByVertex;
            _edgeMultimapConcept = edgeMultimapConcept;
        }

        public int VertexCount
        {
            get
            {
                ReadOnlyIncidenceGraph<
                    TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> self = this;
                return self._outEdgesByVertex is null ? 0 : self.GetCountUnchecked();
            }
        }

        public int EdgeCount => _headByEdge is { } headByEdge ? headByEdge.Count : 0;

        /// <inheritdoc/>
        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _tailByEdge.TryGetValue(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _headByEdge.TryGetValue(edge, out head);

        /// <inheritdoc/>
        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex)
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> self = this;
            return self._edgeMultimapConcept.EnumerateValues(self._outEdgesByVertex, vertex);
        }

        /// <inheritdoc/>
        public AdjacencyEnumerator<TVertex, TEdge,
            ReadOnlyIncidenceGraph<TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept>,
            TEdgeEnumerator> EnumerateOutNeighbors(TVertex vertex)
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> self = this;
            TEdgeEnumerator edgeEnumerator = self.EnumerateOutEdges(vertex);
            return AdjacencyEnumeratorFactory<TVertex, TEdge>.Create(self, edgeEnumerator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetCountUnchecked()
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapConcept> self = this;
            return self._edgeMultimapConcept.GetCount(self._outEdgesByVertex);
        }
    }
}
