namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class IncidenceGraphFactory<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : notnull
    {
        public static IncidenceGraph<TVertex, TEdge, Dictionary<TEdge, TVertex>, Dictionary<TVertex, List<TEdge>>>
            Create()
        {
            Dictionary<TEdge, TVertex> tailByEdge = new();
            Dictionary<TEdge, TVertex> headByEdge = new();
            Dictionary<TVertex, List<TEdge>> outEdgesByVertex = new();
            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }

        public static IncidenceGraph<TVertex, TEdge, Dictionary<TEdge, TVertex>, Dictionary<TVertex, List<TEdge>>>
            Create(IEqualityComparer<TVertex>? vertexComparer, IEqualityComparer<TEdge>? edgeComparer)
        {
            Dictionary<TEdge, TVertex> tailByEdge = new(edgeComparer);
            Dictionary<TEdge, TVertex> headByEdge = new(edgeComparer);
            Dictionary<TVertex, List<TEdge>> outEdgesByVertex = new(vertexComparer);
            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }

        public static IncidenceGraph<TVertex, TEdge, TEndpointMap, TEdgesMap> CreateUnchecked<TEndpointMap, TEdgesMap>(
            TEndpointMap tailByEdge, TEndpointMap headByEdge, TEdgesMap outEdgesByVertex)
            where TEndpointMap : IDictionary<TEdge, TVertex>, IReadOnlyDictionary<TEdge, TVertex>
            where TEdgesMap : IDictionary<TVertex, List<TEdge>>, IReadOnlyDictionary<TVertex, List<TEdge>>
        {
            if (tailByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(tailByEdge));
            if (headByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(headByEdge));
            if (outEdgesByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(outEdgesByVertex));

            return new(tailByEdge, headByEdge, outEdgesByVertex);
        }
    }
}
