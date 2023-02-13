namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct EdgeIdentityGraph<TVertex, TEdge, TEndpointMap, TEdgesMap> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, List<TEdge>.Enumerator>,
        IAdjacency<TVertex, IEnumerator<TVertex>>
        where TEndpointMap : IDictionary<TEdge, TVertex>
        where TEdgesMap : IDictionary<TVertex, List<TEdge>>, IReadOnlyDictionary<TVertex, List<TEdge>>
    {
        private readonly TEndpointMap _tailByEdge;
        private readonly TEndpointMap _headByEdge;
        private readonly TEdgesMap _outEdgesByVertex;

        internal EdgeIdentityGraph(TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgesMap outEdgesByVertex)
        {
            _tailByEdge = tailByEdge;
            _headByEdge = headByEdge;
            _outEdgesByVertex = outEdgesByVertex;
        }

        public static EdgeIdentityGraph<TVertex, TEdge, TEndpointMap, TEdgesMap> CreateUnchecked(
            TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgesMap outEdgesByVertex)
        {
            if (tailByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(tailByEdge));
            if (headByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));

            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }

        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _tailByEdge.TryGetValue(edge, out tail);

        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _headByEdge.TryGetValue(edge, out head);

        public List<TEdge>.Enumerator EnumerateOutEdges(TVertex vertex) =>
            EdgesMapHelpers<List<TEdge>, List<TEdge>.Enumerator>.EnumerateOutEdges(
                _outEdgesByVertex, vertex, default(ListEnumerablePolicy<TEdge>));

        public IEnumerator<TVertex> EnumerateNeighbors(TVertex vertex)
        {
            List<TEdge>.Enumerator edgeEnumerator = EnumerateOutEdges(vertex);
            while (edgeEnumerator.MoveNext())
            {
                if (TryGetHead(edgeEnumerator.Current, out TVertex? neighbor))
                    yield return neighbor;
            }
        }

        public bool TryAdd(TEdge edge, TVertex tail, TVertex head)
        {
            if (!DictionaryExtensions.TryAdd(_tailByEdge, edge, tail))
                return false;

            if (TryGetValue(_outEdgesByVertex, tail, out List<TEdge>? outEdges))
            {
                outEdges.Add(edge);
            }
            else
            {
                outEdges = new(1) { edge };
                _outEdgesByVertex.Add(tail, outEdges);
            }

            // ReSharper disable once PossibleStructMemberModificationOfNonVariableStruct
            _headByEdge[edge] = head;
            return true;
        }

        private static bool TryGetValue<TDictionary>(
            TDictionary dictionary, TVertex vertex, [NotNullWhen(true)] out List<TEdge>? value)
            where TDictionary : IReadOnlyDictionary<TVertex, List<TEdge>> =>
            dictionary.TryGetValue(vertex, out value);
    }
}
