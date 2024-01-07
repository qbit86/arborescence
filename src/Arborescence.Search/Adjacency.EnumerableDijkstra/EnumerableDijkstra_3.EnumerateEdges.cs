#if NET6_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System;
    using System.Collections.Generic;

    public static partial class EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
    {
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, weightMonoid);

        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex, weightMonoid);

        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
            where TWeightComparer : IComparer<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            if (weightMonoid is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightMonoid));

            Dictionary<TVertex, TWeight> distanceByVertex = new();
            Comparer<TWeight> weightComparer = Comparer<TWeight>.Default;
            return EnumerateEdgesUnchecked(graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            if (distanceByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(distanceByVertex));

            if (weightMonoid is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightMonoid));

            Comparer<TWeight> weightComparer = Comparer<TWeight>.Default;
            return EnumerateEdgesUnchecked(graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            if (distanceByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(distanceByVertex));

            if (weightMonoid is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightMonoid));

            if (weightComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightComparer));

            return EnumerateEdgesUnchecked(graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesUnchecked<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            PriorityQueue<TVertex, TWeight> frontier = new();
            return EnumerateEdgesIterator(
                graph, source, weightByEdge, frontier, distanceByVertex, weightMonoid, weightComparer);
        }

        private static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            PriorityQueue<TVertex, TWeight> frontier, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            distanceByVertex[source] = weightMonoid.Identity;
            frontier.Enqueue(source, weightMonoid.Identity);

            while (frontier.TryDequeue(out TVertex? current, out TWeight? priority))
            {
                if (!distanceByVertex.TryGetValue(current, out TWeight? currentDistance))
                    throw new InvalidOperationException(nameof(distanceByVertex));
                if (weightComparer.Compare(priority, currentDistance) > 0)
                    continue;
                TNeighborEnumerator neighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        TVertex neighbor = neighbors.Current;
                        Endpoints<TVertex> edge = new(current, neighbor);
                        if (!weightByEdge.TryGetValue(edge, out TWeight? weight))
                            throw new InvalidOperationException(nameof(weightByEdge));
                        TWeight neighborDistanceCandidate = weightMonoid.Combine(currentDistance, weight);
                        if (!distanceByVertex.TryGetValue(neighbor, out TWeight? neighborDistance) ||
                            weightComparer.Compare(neighborDistanceCandidate, neighborDistance) < 0)
                        {
                            yield return edge;
                            distanceByVertex[neighbor] = neighborDistanceCandidate;
                            frontier.Enqueue(neighbor, neighborDistanceCandidate);
                        }
                    }
                }
                finally
                {
                    neighbors.Dispose();
                }
            }
        }
    }
}
#endif
