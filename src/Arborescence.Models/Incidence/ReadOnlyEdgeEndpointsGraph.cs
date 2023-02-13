namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct ReadOnlyEdgeEndpointsGraph<
        TVertex, TEdgesMap, TEdgeCollection, TEdgeEnumerator, TEdgeCollectionPolicy> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>,
        IAdjacency<TVertex, IEnumerator<TVertex>>
        where TEdgesMap : IReadOnlyDictionary<TVertex, TEdgeCollection>
        where TEdgeEnumerator : IEnumerator<Endpoints<TVertex>>
        where TEdgeCollectionPolicy : IEnumerablePolicy<TEdgeCollection, TEdgeEnumerator>
    {
        private readonly TEdgesMap _outEdgesByVertex;
        private readonly TEdgeCollectionPolicy _edgeCollectionPolicy;

        private ReadOnlyEdgeEndpointsGraph(TEdgesMap outEdgesByVertex, TEdgeCollectionPolicy edgeCollectionPolicy)
        {
            _outEdgesByVertex = outEdgesByVertex;
            _edgeCollectionPolicy = edgeCollectionPolicy;
        }

        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            throw new System.NotImplementedException();

        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            throw new System.NotImplementedException();

        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex) => throw new System.NotImplementedException();

        public IEnumerator<TVertex> EnumerateNeighbors(TVertex vertex) => throw new System.NotImplementedException();
    }
}
