namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct ReadOnlyIncidenceGraph<
        TVertex, TEdge, TEndpointMap, TEdgesMap, TEdgeCollection, TEdgeEnumerator, TEdgeCollectionPolicy> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>,
        IAdjacency<TVertex, IEnumerator<TVertex>>
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

        public IEnumerator<TVertex> EnumerateNeighbors(TVertex vertex)
        {
            TEdgeEnumerator edgeEnumerator = EnumerateOutEdges(vertex);
            while (edgeEnumerator.MoveNext())
            {
                if (TryGetHead(edgeEnumerator.Current, out TVertex? neighbor))
                    yield return neighbor;
            }
        }
    }
}
