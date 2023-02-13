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

        public void Add(TVertex tail, TVertex head)
        {
            Endpoints<TVertex> edge = new(tail, head);
            if (TryGetValue(_outEdgesByVertex, tail, out List<Endpoints<TVertex>>? outEdges))
            {
                outEdges.Add(edge);
            }
            else
            {
                outEdges = new(1) { edge };
                _outEdgesByVertex.Add(tail, outEdges);
            }
        }

        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out List<Endpoints<TVertex>>? value)
            where TDictionary : IReadOnlyDictionary<TVertex, List<Endpoints<TVertex>>> =>
            dictionary.TryGetValue(vertex, out value);
    }
}
