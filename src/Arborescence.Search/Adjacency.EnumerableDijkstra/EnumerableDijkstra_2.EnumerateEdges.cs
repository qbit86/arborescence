namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableDijkstra<TVertex, TWeight>
    {
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerableDijkstra<TVertex, IEnumerator<TVertex>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, weightMonoid);

        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerableDijkstra<TVertex, IEnumerator<TVertex>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, distanceByVertex, weightMonoid);

        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
            where TWeightComparer : IComparer<TWeight> =>
            EnumerableDijkstra<TVertex, IEnumerator<TVertex>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);
    }
}
