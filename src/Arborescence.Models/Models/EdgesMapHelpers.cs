namespace Arborescence.Models
{
    using System.Collections.Generic;

    internal static class EdgesMapHelpers<TEdgeCollection, TEdgeEnumerator>
    {
        internal static TEdgeEnumerator EnumerateOutEdges<TVertex, TEdgesMap, TEnumerablePolicy>(
            TEdgesMap outEdgesByVertex, TVertex vertex, TEnumerablePolicy enumerablePolicy = default!)
            where TEdgesMap : IReadOnlyDictionary<TVertex, TEdgeCollection>
            where TEnumerablePolicy : IEnumerablePolicy<TEdgeCollection, TEdgeEnumerator>
            => outEdgesByVertex.TryGetValue(vertex, out TEdgeCollection? outEdges)
                ? enumerablePolicy.GetEnumerator(outEdges)
                : enumerablePolicy.GetEmptyEnumerator();
    }
}
