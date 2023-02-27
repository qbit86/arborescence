namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    public readonly partial struct ReadOnlyAdjacencyGraph<
        TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, TVertexEnumerator>>,
        IOutNeighborsAdjacency<TVertex, TVertexEnumerator>,
        IEquatable<ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TVertexMultimapPolicy : IReadOnlyMultimapPolicy<TVertex, TVertexMultimap, TVertexEnumerator>
    {
        private readonly TVertexMultimap _neighborsByVertex;
        private readonly TVertexMultimapPolicy _vertexMultimapPolicy;

        internal ReadOnlyAdjacencyGraph(TVertexMultimap neighborsByVertex, TVertexMultimapPolicy vertexMultimapPolicy)
        {
            _neighborsByVertex = neighborsByVertex;
            _vertexMultimapPolicy = vertexMultimapPolicy;
        }

        public int VertexCount
        {
            get
            {
                ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> self = this;
                return self.IsDefault ? 0 : self.GetCountUnchecked();
            }
        }

        private bool IsDefault => _neighborsByVertex is null;

        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        public IncidenceEnumerator<TVertex, TVertexEnumerator> EnumerateOutEdges(TVertex vertex)
        {
            TVertexEnumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return IncidenceEnumerator.Create(vertex, neighborEnumerator);
        }

        public TVertexEnumerator EnumerateOutNeighbors(TVertex vertex)
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> self = this;
            return self._vertexMultimapPolicy.EnumerateValues(self._neighborsByVertex, vertex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetCountUnchecked()
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapPolicy> self = this;
            return self._vertexMultimapPolicy.GetCount(self._neighborsByVertex);
        }
    }
}
