#if NET6_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, weightMonoid);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex, weightMonoid);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            var weightComparer = Comparer<TWeight>.Default;
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

            var weightComparer = Comparer<TWeight>.Default;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

            while (frontier.TryDequeue(out var current, out var priority))
            {
                if (!distanceByVertex.TryGetValue(current, out var currentDistance))
                    ThrowHelper<TVertex>.ThrowVertexNotFoundException(current);
                if (weightComparer.Compare(priority, currentDistance) > 0)
                    continue;
                var neighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        var neighbor = neighbors.Current;
                        Endpoints<TVertex> edge = new(current, neighbor);
                        if (!weightByEdge.TryGetValue(edge, out var weight))
                            continue;
                        var neighborDistanceCandidate = weightMonoid.Combine(currentDistance, weight);
                        if (!distanceByVertex.TryGetValue(neighbor, out var neighborDistance) ||
                            weightComparer.Compare(neighborDistanceCandidate, neighborDistance) < 0)
                        {
                            // In traversal algorithms, we raise the “tree edge” event before updating the color map.
                            distanceByVertex[neighbor] = neighborDistanceCandidate;
                            // But in search algorithms, we raise the “relaxed edge” event after updating the distance map.
                            yield return edge;
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
