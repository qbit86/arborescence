namespace Arborescence.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public readonly struct IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, List<TEdge>.Enumerator>,
        IOutNeighborsAdjacency<TVertex,
            AdjacencyEnumerator<TVertex, TEdge, IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>,
                List<TEdge>.Enumerator>>
        where TEndpointMap : IDictionary<TEdge, TVertex>
        where TEdgeMultimap : IDictionary<TVertex, List<TEdge>>, IReadOnlyDictionary<TVertex, List<TEdge>>
    {
        private static readonly ListDictionaryMultimapPolicy<TVertex, TEdge, TEdgeMultimap> s_multimapPolicy = default;

        private readonly TEndpointMap _tailByEdge;
        private readonly TEndpointMap _headByEdge;
        private readonly TEdgeMultimap _outEdgesByVertex;

        internal IncidenceGraph(TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgeMultimap outEdgesByVertex)
        {
            _tailByEdge = tailByEdge;
            _headByEdge = headByEdge;
            _outEdgesByVertex = outEdgesByVertex;
        }

        public int VertexCount
        {
            get
            {
                IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
                return self._outEdgesByVertex is null ? 0 : GetCountUnchecked(self._outEdgesByVertex);
            }
        }

        public int EdgeCount
        {
            get
            {
                IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
                return self._headByEdge is null ? 0 : self._headByEdge.Count;
            }
        }

        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _tailByEdge.TryGetValue(edge, out tail);

        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _headByEdge.TryGetValue(edge, out head);

        public List<TEdge>.Enumerator EnumerateOutEdges(TVertex vertex) =>
            s_multimapPolicy.GetEnumerator(_outEdgesByVertex, vertex);

        public AdjacencyEnumerator<
                TVertex, TEdge, IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap>, List<TEdge>.Enumerator>
            EnumerateOutNeighbors(TVertex vertex)
        {
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            List<TEdge>.Enumerator edgeEnumerator = self.EnumerateOutEdges(vertex);
            return AdjacencyEnumerator<TVertex, TEdge>.Create(self, edgeEnumerator);
        }

        public bool TryAdd(TEdge edge, TVertex tail, TVertex head)
        {
            IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgeMultimap> self = this;
            if (!self._tailByEdge.TryAddStrict(edge, tail))
                return false;

            if (TryGetValue(self._outEdgesByVertex, tail, out List<TEdge>? outEdges))
            {
                outEdges.Add(edge);
            }
            else
            {
                outEdges = new(1) { edge };
                self._outEdgesByVertex.Add(tail, outEdges);
            }

            // ReSharper disable once PossibleStructMemberModificationOfNonVariableStruct
            self._headByEdge[edge] = head;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out List<TEdge>? value)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TEdge>> =>
            dictionary.TryGetValue(vertex, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetCountUnchecked<TDictionary>(TDictionary dictionary)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TEdge>> => dictionary.Count;
    }
}
