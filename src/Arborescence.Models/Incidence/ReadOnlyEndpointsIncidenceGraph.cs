namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using static TryHelpers;

    public readonly struct ReadOnlyEndpointsIncidenceGraph<
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

        internal ReadOnlyEndpointsIncidenceGraph(TEdgesMap outEdgesByVertex, TEdgeCollectionPolicy edgeCollectionPolicy)
        {
            _outEdgesByVertex = outEdgesByVertex;
            _edgeCollectionPolicy = edgeCollectionPolicy;
        }

        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex) =>
            EdgesMapHelpers<TEdgeCollection, TEdgeEnumerator>.EnumerateOutEdges(
                _outEdgesByVertex, vertex, _edgeCollectionPolicy);

        public IEnumerator<TVertex> EnumerateNeighbors(TVertex vertex)
        {
            TEdgeEnumerator edgeEnumerator = EnumerateOutEdges(vertex);
            while (edgeEnumerator.MoveNext())
                yield return edgeEnumerator.Current.Head;
        }
    }
}
