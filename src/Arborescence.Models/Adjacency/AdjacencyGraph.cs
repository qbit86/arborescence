namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    public readonly partial struct AdjacencyGraph<TVertex, TVertexMultimap> :
        ITailIncidence<TVertex, Endpoints<TVertex>>,
        IHeadIncidence<TVertex, Endpoints<TVertex>>,
        IOutEdgesIncidence<TVertex, IncidenceEnumerator<TVertex, List<TVertex>.Enumerator>>,
        IOutNeighborsAdjacency<TVertex, List<TVertex>.Enumerator>,
        IEquatable<AdjacencyGraph<TVertex, TVertexMultimap>>
        where TVertexMultimap : IDictionary<TVertex, List<TVertex>>, IReadOnlyDictionary<TVertex, List<TVertex>>
    {
        private static ListDictionaryMultimapPolicy<TVertex, TVertexMultimap> MultimapPolicy => default;

        private readonly TVertexMultimap _neighborsByVertex;

        internal AdjacencyGraph(TVertexMultimap neighborsByVertex) => _neighborsByVertex = neighborsByVertex;

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

        public IncidenceEnumerator<TVertex, List<TVertex>.Enumerator> EnumerateOutEdges(TVertex vertex)
        {
            List<TVertex>.Enumerator neighborEnumerator = EnumerateOutNeighbors(vertex);
            return new(vertex, neighborEnumerator);
        }

        public List<TVertex>.Enumerator EnumerateOutNeighbors(TVertex vertex) =>
            MultimapPolicy.GetEnumerator(_neighborsByVertex, vertex);

        public void AddEdge(TVertex tail, TVertex head)
        {
            AdjacencyGraph<TVertex, TVertexMultimap> self = this;
            if (TryGetValue(self._neighborsByVertex, tail, out List<TVertex>? neighbors))
            {
                neighbors.Add(head);
            }
            else
            {
                neighbors = new(1) { head };
                self._neighborsByVertex.Add(tail, neighbors);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out List<TVertex>? value)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TVertex>> =>
            dictionary.TryGetValue(vertex, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetCountUnchecked<TDictionary>(TDictionary dictionary)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TVertex>> => dictionary.Count;
    }
}
