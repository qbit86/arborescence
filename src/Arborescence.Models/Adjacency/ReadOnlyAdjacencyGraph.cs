namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    public readonly struct ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, TVertexEnumerator>>,
        IAdjacency<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TVertexMultimapPolicy : IMultimapPolicy<TVertex, TVertexMultimap, TVertexEnumerator>
    {
        private readonly TVertexMultimap _neighborsByVertex;
        private readonly TVertexMultimapPolicy _vertexMultimapPolicy;

        internal ReadOnlyAdjacencyGraph(TVertexMultimap neighborsByVertex, TVertexMultimapPolicy vertexMultimapPolicy)
        {
            _neighborsByVertex = neighborsByVertex;
            _vertexMultimapPolicy = vertexMultimapPolicy;
        }

        public int VertexCount => _neighborsByVertex is null || _vertexMultimapPolicy is null ? 0 : GetCountUnchecked();

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
            _vertexMultimapPolicy.GetEnumerator(_neighborsByVertex, vertex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetCountUnchecked() => _vertexMultimapPolicy.GetCount(_neighborsByVertex);
    }
}
