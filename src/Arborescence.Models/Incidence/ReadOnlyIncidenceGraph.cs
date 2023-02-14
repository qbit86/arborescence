namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct ReadOnlyIncidenceGraph<
        TVertex, TEdge, TEndpointMap, TEdgesMap, TEdgeCollection, TEdgeEnumerator, TEdgeCollectionPolicy> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>,
        IAdjacency<TVertex, AdjacencyEnumerator<
            TVertex, TEdge,
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEndpointMap, TEdgesMap, TEdgeCollection, TEdgeEnumerator, TEdgeCollectionPolicy>,
            TEdgeEnumerator>>
        where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
        where TEdgesMap : IReadOnlyDictionary<TVertex, TEdgeCollection>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TEdgeCollectionPolicy : IEnumerablePolicy<TEdgeCollection, TEdgeEnumerator>
    {
        private readonly TEndpointMap _tailByEdge;
        private readonly TEndpointMap _headByEdge;
        private readonly TEdgesMap _outEdgesByVertex;
        private readonly TEdgeCollectionPolicy _edgeCollectionPolicy;

        internal ReadOnlyIncidenceGraph(
            TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgesMap outEdgesByVertex,
            TEdgeCollectionPolicy edgeCollectionPolicy)
        {
            _tailByEdge = tailByEdge;
            _headByEdge = headByEdge;
            _outEdgesByVertex = outEdgesByVertex;
            _edgeCollectionPolicy = edgeCollectionPolicy;
        }

        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _tailByEdge.TryGetValue(edge, out tail);

        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _headByEdge.TryGetValue(edge, out head);

        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex) =>
            MultimapHelpers<TEdgeCollection, TEdgeEnumerator>.Enumerate(
                _outEdgesByVertex, vertex, _edgeCollectionPolicy);

        public AdjacencyEnumerator<TVertex, TEdge,
            ReadOnlyIncidenceGraph<
                TVertex, TEdge, TEndpointMap, TEdgesMap, TEdgeCollection, TEdgeEnumerator, TEdgeCollectionPolicy>,
            TEdgeEnumerator> EnumerateNeighbors(TVertex vertex)
        {
            TEdgeEnumerator edgeEnumerator = EnumerateOutEdges(vertex);
            return AdjacencyEnumerator<TVertex, TEdge>.Create(this, edgeEnumerator);
        }
    }
}
