namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal static class EdgesMapHelpers<TEdgeCollection, TEdgeEnumerator>
    {
        internal static TEdgeEnumerator EnumerateOutEdges<TVertex, TEdgesMap, TEdgeCollectionPolicy>(
            TEdgesMap outEdgesByVertex, TVertex vertex, TEdgeCollectionPolicy policy = default!)
            where TEdgesMap : IReadOnlyDictionary<TVertex, TEdgeCollection>
            where TEdgeCollectionPolicy : IEnumerablePolicy<TEdgeCollection, TEdgeEnumerator>
            => outEdgesByVertex.TryGetValue(vertex, out TEdgeCollection? outEdges)
                ? policy.GetEnumerator(outEdges)
                : policy.GetEmptyEnumerator();
    }
}
