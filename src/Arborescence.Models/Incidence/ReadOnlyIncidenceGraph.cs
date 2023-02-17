namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct ReadOnlyIncidenceGraph<
        TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>,
        IAdjacency<TVertex, AdjacencyEnumerator<
            TVertex, TEdge,
            ReadOnlyIncidenceGraph<TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>,
            TEdgeEnumerator>>
        where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TEdgeMultimapPolicy : IMultimapPolicy<TVertex, TEdgeMultimap, TEdgeEnumerator>
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

        public int EdgeCount => _headByEdge is null ? 0 : _headByEdge.Count;

        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _tailByEdge.TryGetValue(edge, out tail);

        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _headByEdge.TryGetValue(edge, out head);

        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex) =>
            _edgeMultimapPolicy.GetEnumerator(_outEdgesByVertex, vertex);

        public AdjacencyEnumerator<TVertex, TEdge,
            ReadOnlyIncidenceGraph<TVertex, TEdge, TEdgeEnumerator, TEndpointMap, TEdgeMultimap, TEdgeMultimapPolicy>,
            TEdgeEnumerator> EnumerateOutNeighbors(TVertex vertex)
        {
            TEdgeEnumerator edgeEnumerator = EnumerateOutEdges(vertex);
            return AdjacencyEnumerator<TVertex, TEdge>.Create(this, edgeEnumerator);
        }
    }
}
