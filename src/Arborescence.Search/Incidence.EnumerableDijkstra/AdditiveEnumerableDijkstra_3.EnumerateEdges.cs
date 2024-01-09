#if NET7_0_OR_GREATER
namespace Arborescence.Search.Incidence
{
    using System.Collections.Generic;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TEdge, TWeight>
    {
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, TEdge, IEnumerator<TEdge>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge);

        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TWeightMap, TDistanceMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, TEdge, IEnumerator<TEdge>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, distanceByVertex);

        public static IEnumerable<TEdge> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, TEdge, IEnumerator<TEdge>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, distanceByVertex, weightComparer);
    }
}
#endif
