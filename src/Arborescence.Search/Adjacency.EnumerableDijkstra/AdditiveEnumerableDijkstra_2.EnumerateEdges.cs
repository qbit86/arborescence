#if NET7_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TWeight>
    {
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, IEnumerator<TVertex>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge);

        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TWeightMap, TDistanceMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, IEnumerator<TVertex>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, distanceByVertex);

        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, IEnumerator<TVertex>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, distanceByVertex, weightComparer);
    }
}
#endif
