namespace Arborescence.Search.Incidence
{
    using System.Collections.Generic;

    public static partial class EnumerableDijkstra<TVertex, TEdge, TEdgeEnumerator, TWeight>
    {
        public static IEnumerable<TEdge> EnumerateEdges<
            TGraph, TWeightMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            TWeightMonoid weightMonoid)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, weightMonoid);

        public static IEnumerable<TEdge> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex, weightMonoid);

        public static IEnumerable<TEdge> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
            where TWeightComparer : IComparer<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<
            TGraph, TWeightMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            TWeightMonoid weightMonoid)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
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

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
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

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
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

        internal static IEnumerable<TEdge> EnumerateEdgesUnchecked<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightMonoid : IMonoid<TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            PriorityQueue<TVertex, TWeight> frontier = new();
            return EnumerateEdgesIterator(
                graph, source, weightByEdge, frontier, distanceByVertex, weightMonoid, weightComparer);
        }

        private static IEnumerable<TEdge> EnumerateEdgesIterator<
            TGraph, TWeightMap, TDistanceMap, TWeightMonoid, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge,
            PriorityQueue<TVertex, TWeight> frontier, TDistanceMap distanceByVertex,
            TWeightMonoid weightMonoid, TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
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
                var outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        var edge = outEdges.Current;
                        if (!weightByEdge.TryGetValue(edge, out var weight))
                            continue;
                        var neighborDistanceCandidate = weightMonoid.Combine(currentDistance, weight);
                        if (!graph.TryGetHead(edge, out var neighbor))
                            continue;
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
                    outEdges.Dispose();
                }
            }
        }
    }
}
