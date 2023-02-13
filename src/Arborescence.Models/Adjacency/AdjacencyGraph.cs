namespace Arborescence.Models.Adjacency
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
            List<TVertex>.Enumerator neighborEnumerator = EnumerateNeighbors(vertex);
            return new(vertex, neighborEnumerator);
        }

        public List<TVertex>.Enumerator EnumerateNeighbors(TVertex vertex) =>
            MultimapHelpers<List<TVertex>, List<TVertex>.Enumerator>.Enumerate(
                _neighborsByVertex, vertex, default(ListEnumerablePolicy<TVertex>));
    }
}
