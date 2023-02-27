namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public readonly partial struct ReadOnlyIncidenceGraph<
        TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>,
        IOutNeighborsAdjacency<TVertex, AdjacencyEnumerator<
            TVertex, TEdge,
            ReadOnlyIncidenceGraph<TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>,
            TEdgeEnumerator>>,
        IEquatable<ReadOnlyIncidenceGraph<
            TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>>
        where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TEdgeMultimapPolicy : IReadOnlyMultimapPolicy<TVertex, TEdgeMultimap, TEdgeEnumerator>
    {
        private readonly TEndpointMap _tailByEdge;
        private readonly TEndpointMap _headByEdge;
        private readonly TEdgeMultimap _outEdgesByVertex;
        private readonly TEdgeMultimapPolicy _edgeMultimapPolicy;

        internal ReadOnlyIncidenceGraph(
            TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex,
            TEdgeMultimapPolicy edgeMultimapPolicy)
        {
            _tailByEdge = tailByEdge;
            _headByEdge = headByEdge;
            _outEdgesByVertex = outEdgesByVertex;
            _edgeMultimapPolicy = edgeMultimapPolicy;
        }

        public int VertexCount
        {
            get
            {
                ReadOnlyIncidenceGraph<
                    TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> self = this;
                return self._outEdgesByVertex is null ? 0 : self.GetCountUnchecked();
            }
        }

        public int EdgeCount => _headByEdge is { } headByEdge ? headByEdge.Count : 0;

        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _tailByEdge.TryGetValue(edge, out tail);

        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _headByEdge.TryGetValue(edge, out head);

        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex)
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> self = this;
            return self._edgeMultimapPolicy.EnumerateValues(self._outEdgesByVertex, vertex);
        }

        public AdjacencyEnumerator<TVertex, TEdge,
            ReadOnlyIncidenceGraph<TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>,
            TEdgeEnumerator> EnumerateOutNeighbors(TVertex vertex)
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> self = this;
            TEdgeEnumerator edgeEnumerator = self.EnumerateOutEdges(vertex);
            return AdjacencyEnumerator<TVertex, TEdge>.Create(self, edgeEnumerator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetCountUnchecked()
        {
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> self = this;
            return self._edgeMultimapPolicy.GetCount(self._outEdgesByVertex);
        }
    }
}
