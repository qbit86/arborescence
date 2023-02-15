namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using static TryHelpers;

    public readonly struct ReadOnlyAdjacencyGraph<
        TVertex, TVerticesMap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, TVertexEnumerator>>,
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

        public IncidenceEnumerator<TVertex, TVertexEnumerator> EnumerateOutEdges(TVertex vertex)
        {
            TVertexEnumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return IncidenceEnumerator.Create(vertex, neighborEnumerator);
        }

        public TVertexEnumerator EnumerateOutNeighbors(TVertex vertex) =>
            MultimapHelpers<TVertexCollection, TVertexEnumerator>.Enumerate(
                _neighborsByVertex, vertex, _vertexCollectionPolicy);
    }
}