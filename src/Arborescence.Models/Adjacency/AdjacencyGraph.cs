namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    public readonly partial struct AdjacencyGraph<
        TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, TVertexEnumerator>>,
        IOutNeighborsAdjacency<TVertex, TVertexEnumerator>,
        IEquatable<AdjacencyGraph<
            TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>>
        where TVertexMultimap : IDictionary<TVertex, TVertexCollection>, IReadOnlyDictionary<TVertex, TVertexCollection>
        where TVertexCollection : ICollection<TVertex>, new()
        where TVertexEnumerator : IEnumerator<TVertex>
        where TVertexCollectionPolicy : IEnumerablePolicy<TVertexCollection, TVertexEnumerator>
    {
        private readonly TVertexMultimap _neighborsByVertex;
        private readonly TVertexCollectionPolicy _vertexCollectionPolicy;

        internal AdjacencyGraph(TVertexMultimap neighborsByVertex, TVertexCollectionPolicy vertexCollectionPolicy)
        {
            _neighborsByVertex = neighborsByVertex;
            _vertexCollectionPolicy = vertexCollectionPolicy;
        }

        public int VertexCount
        {
            get
            {
                TVertexMultimap? neighborsByVertex = _neighborsByVertex;
                return neighborsByVertex is null ? 0 : GetCountUnchecked(neighborsByVertex);
            }
        }

        public bool TryGetTail(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex tail) =>
            Some(edge.Tail, out tail);

        public bool TryGetHead(Endpoints<TVertex> edge, [MaybeNullWhen(false)] out TVertex head) =>
            Some(edge.Head, out head);

        public IncidenceEnumerator<TVertex, TVertexEnumerator> EnumerateOutEdges(TVertex vertex)
        {
            TVertexEnumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return new(vertex, neighborEnumerator);
        }

        public void AddEdge(TVertex tail, TVertex head)
        {
            AdjacencyGraph<TVertex, TVertexMultimap, TVertexCollection, TVertexEnumerator, TVertexCollectionPolicy>
                self = this;
            if (TryGetValue(self._neighborsByVertex, tail, out TVertexCollection? neighbors))
            {
                neighbors.Add(head);
            }
            else
            {
                neighbors = new() { head };
                self._neighborsByVertex.Add(tail, neighbors);
            }
        }

        public TVertexEnumerator EnumerateOutNeighbors(TVertex vertex) =>
            TryGetValue(_neighborsByVertex, vertex, out TVertexCollection? values)
                ? _vertexCollectionPolicy.GetEnumerator(values)
                : _vertexCollectionPolicy.GetEmptyEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out TVertexCollection? value)
            where TDictionary : IReadOnlyDictionary<TVertex, TVertexCollection> =>
            dictionary.TryGetValue(vertex, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetCountUnchecked<TDictionary>(TDictionary dictionary)
            where TDictionary : IReadOnlyDictionary<TVertex, TVertexCollection> => dictionary.Count;
    }
}
