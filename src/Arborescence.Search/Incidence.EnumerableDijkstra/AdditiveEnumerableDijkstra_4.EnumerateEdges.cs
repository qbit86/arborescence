#if NET7_0_OR_GREATER
namespace Arborescence.Search.Incidence
{
    using System.Collections.Generic;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TEdge, TEdgeEnumerator, TWeight>
    {
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge);

        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TWeightMap, TDistanceMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex);

        public static IEnumerable<TEdge> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex, weightComparer);

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            Dictionary<TVertex, TWeight> distanceByVertex = new();
            Comparer<TWeight> weightComparer = Comparer<TWeight>.Default;
            return EnumerateEdgesUnchecked(
                graph, source, weightByEdge, distanceByVertex, weightComparer);
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            if (distanceByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(distanceByVertex));

            Comparer<TWeight> weightComparer = Comparer<TWeight>.Default;
            return EnumerateEdgesUnchecked(
                graph, source, weightByEdge, distanceByVertex, weightComparer);
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            if (distanceByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(distanceByVertex));

            if (weightComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightComparer));

            return EnumerateEdgesUnchecked(
                graph, source, weightByEdge, distanceByVertex, weightComparer);
        }

        private static IEnumerable<TEdge> EnumerateEdgesUnchecked<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            AdditiveMonoid<TWeight> weightMonoid = default;
            return EnumerableDijkstra<TVertex, TEdge, TEdgeEnumerator, TWeight>.EnumerateEdgesUnchecked(
                graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);
        }
    }
}
#endif
