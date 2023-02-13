namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using static TryHelpers;

    public readonly struct EndpointsIncidenceGraph<TVertex, TEdgesMap> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, List<Endpoints<TVertex>>.Enumerator>,
        IAdjacency<TVertex, IEnumerator<TVertex>>
        where TEdgesMap :
        IDictionary<TVertex, List<Endpoints<TVertex>>>, IReadOnlyDictionary<TVertex, List<Endpoints<TVertex>>>
    {
        private readonly TEdgesMap _outEdgesByVertex;

        internal EndpointsIncidenceGraph(TEdgesMap outEdgesByVertex) => _outEdgesByVertex = outEdgesByVertex;

        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        public List<Endpoints<TVertex>>.Enumerator EnumerateOutEdges(TVertex vertex) =>
            EdgesMapHelpers<List<Endpoints<TVertex>>, List<Endpoints<TVertex>>.Enumerator>.EnumerateOutEdges(
                _outEdgesByVertex, vertex, default(ListEnumerablePolicy<Endpoints<TVertex>>));

        public IEnumerator<TVertex> EnumerateNeighbors(TVertex vertex)
        {
            List<Endpoints<TVertex>>.Enumerator edgeEnumerator = EnumerateOutEdges(vertex);
            while (edgeEnumerator.MoveNext())
                yield return edgeEnumerator.Current.Head;
        }
    }
}
