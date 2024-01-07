#if NET7_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
    {
        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            PriorityQueue<TVertex, TWeight> frontier = new();
            Dictionary<TVertex, TWeight> distanceByVertex = new();
            AdditiveMonoid<TWeight> weightMonoid = default;
            Comparer<TWeight> weightComparer = Comparer<TWeight>.Default;
            return EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>.EnumerateEdgesIterator(
                graph, source, weightByEdge, frontier, distanceByVertex, weightMonoid, weightComparer);
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            PriorityQueue<TVertex, TWeight> frontier = new();
            Dictionary<TVertex, TWeight> distanceByVertex = new();
            AdditiveMonoid<TWeight> weightMonoid = default;
            return EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>.EnumerateEdgesIterator(
                graph, source, weightByEdge, frontier, distanceByVertex, weightMonoid, weightComparer);
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            TDistanceMap distanceByVertex, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (distanceByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            PriorityQueue<TVertex, TWeight> frontier = new();
            AdditiveMonoid<TWeight> weightMonoid = default;
            return EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>.EnumerateEdgesIterator(
                graph, source, weightByEdge, frontier, distanceByVertex, weightMonoid, weightComparer);
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            PriorityQueue<TVertex, TWeight> frontier, TDistanceMap distanceByVertex, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (distanceByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            AdditiveMonoid<TWeight> weightMonoid = default;
            return EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>.EnumerateEdgesIterator(
                graph, source, weightByEdge, frontier, distanceByVertex, weightMonoid, weightComparer);
        }
    }
}
#endif
