namespace Arborescence.Models.Incidence
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct EdgeIdentityGraph<TVertex, TEdge, TEndpointMap, TEdgesMap> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, List<TEdge>.Enumerator>,
        IAdjacency<TVertex, IEnumerator<TVertex>>
        where TEndpointMap : IReadOnlyDictionary<TEdge, TVertex>
        where TEdgesMap : IReadOnlyDictionary<TVertex, List<TEdge>>
    {
        private static readonly List<TEdge> s_empty = new();
        private readonly TEndpointMap _tailByEdge;
        private readonly TEndpointMap _headByEdge;
        private readonly TEdgesMap _outEdgesByVertex;

        private EdgeIdentityGraph(TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgesMap outEdgesByVertex)
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
            _outEdgesByVertex.TryGetValue(vertex, out List<TEdge>? outEdges)
                ? outEdges.GetEnumerator()
                : s_empty.GetEnumerator();

        public IEnumerator<TVertex> EnumerateNeighbors(TVertex vertex)
        {
            List<TEdge>.Enumerator edgeEnumerator = EnumerateOutEdges(vertex);
            while (edgeEnumerator.MoveNext())
            {
                if (TryGetHead(edgeEnumerator.Current, out TVertex? neighbor))
                    yield return neighbor;
            }
        }
    }
}
