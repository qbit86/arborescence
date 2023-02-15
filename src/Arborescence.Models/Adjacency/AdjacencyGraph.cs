namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using static TryHelpers;

    public readonly struct AdjacencyGraph<TVertex, TVerticesMap> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, List<TVertex>.Enumerator>>,
        IAdjacency<TVertex, List<TVertex>.Enumerator>
        where TVerticesMap : IDictionary<TVertex, List<TVertex>>, IReadOnlyDictionary<TVertex, List<TVertex>>
    {
        private readonly TVerticesMap _neighborsByVertex;

        internal AdjacencyGraph(TVerticesMap neighborsByVertex) => _neighborsByVertex = neighborsByVertex;

        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        public IncidenceEnumerator<TVertex, List<TVertex>.Enumerator> EnumerateOutEdges(TVertex vertex)
        {
            List<TVertex>.Enumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return new(vertex, neighborEnumerator);
        }

        public List<TVertex>.Enumerator EnumerateOutNeighbors(TVertex vertex) =>
            MultimapHelpers<List<TVertex>, List<TVertex>.Enumerator>.Enumerate(
                _neighborsByVertex, vertex, default(ListEnumerablePolicy<TVertex>));

        public void Add(TVertex tail, TVertex head)
        {
            if (TryGetValue(_neighborsByVertex, tail, out List<TVertex>? neighbors))
            {
                neighbors.Add(head);
            }
            else
            {
                neighbors = new(1) { head };
                _neighborsByVertex.Add(tail, neighbors);
            }
        }

        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out List<TVertex>? value)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TVertex>> =>
            dictionary.TryGetValue(vertex, out value);
    }
}