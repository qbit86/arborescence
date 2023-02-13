namespace Arborescence.Models.Adjacency
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using static TryHelpers;

    public readonly struct ReadOnlyAdjacencyGraph<
        TVertex, TVerticesMap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IEnumerator<Endpoints<TVertex>>>,
        IAdjacency<TVertex, TVertexEnumerator>
        where TVerticesMap : IReadOnlyDictionary<TVertex, TVertexCollection>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TVertexCollectionPolicy : IEnumerablePolicy<TVertexCollection, TVertexEnumerator>
    {
        private readonly TVerticesMap _neighborsByVertex;
        private readonly TVertexCollectionPolicy _vertexCollectionPolicy;

        internal ReadOnlyAdjacencyGraph(TVerticesMap neighborsByVertex, TVertexCollectionPolicy vertexCollectionPolicy)
        {
            _neighborsByVertex = neighborsByVertex;
            _vertexCollectionPolicy = vertexCollectionPolicy;
        }

        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        public IEnumerator<Endpoints<TVertex>> EnumerateOutEdges(TVertex vertex)
        {
            TVertexEnumerator vertexEnumerator = EnumerateNeighbors(vertex);
            while (vertexEnumerator.MoveNext())
                yield return new(vertex, vertexEnumerator.Current);
        }

        public TVertexEnumerator EnumerateNeighbors(TVertex vertex) =>
            _neighborsByVertex.TryGetValue(vertex, out TVertexCollection? neighbors)
                ? _vertexCollectionPolicy.GetEnumerator(neighbors)
                : _vertexCollectionPolicy.GetEmptyEnumerator();
    }
}
