namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    public readonly partial struct ReadOnlyAdjacencyGraph<
        TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, TVertexEnumerator>>,
        IOutNeighborsAdjacency<TVertex, TVertexEnumerator>,
        IEquatable<ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept>>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TVertexMultimapConcept : IReadOnlyMultimapConcept<TVertexMultimap, TVertex, TVertexEnumerator>
    {
        private readonly TVertexMultimap _neighborsByVertex;
        private readonly TVertexMultimapConcept _vertexMultimapConcept;

        internal ReadOnlyAdjacencyGraph(TVertexMultimap neighborsByVertex, TVertexMultimapConcept vertexMultimapConcept)
        {
            _neighborsByVertex = neighborsByVertex;
            _vertexMultimapConcept = vertexMultimapConcept;
        }

        public int VertexCount
        {
            get
            {
                ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
                return self._neighborsByVertex is null ? 0 : self.GetCountUnchecked();
            }
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

        public TVertexEnumerator EnumerateOutNeighbors(TVertex vertex)
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return self._vertexMultimapConcept.EnumerateValues(self._neighborsByVertex, vertex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetCountUnchecked()
        {
            ReadOnlyAdjacencyGraph<TVertex, TVertexEnumerator, TVertexMultimap, TVertexMultimapConcept> self = this;
            return self._vertexMultimapConcept.GetCount(self._neighborsByVertex);
        }
    }
}
