#if NET7_0_OR_GREATER
namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
    {
        /// <summary>
        /// Enumerates edges of the graph in an order defined by the Dijkstra algorithm, starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="weightByEdge">The weight of each edge in the graph.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TWeightMap">The type of the weight map.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge);

        /// <summary>
        /// Enumerates edges of the graph in an order defined by the Dijkstra algorithm, starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="weightByEdge">The weight of each edge in the graph.</param>
        /// <param name="distanceByVertex">The distance to each discovered vertex.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TWeightMap">The type of the weight map.</typeparam>
        /// <typeparam name="TDistanceMap">The type of the distance map.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>,
        /// or <paramref name="distanceByVertex"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TWeightMap, TDistanceMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex);

        /// <summary>
        /// Enumerates edges of the graph in an order defined by the Dijkstra algorithm, starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="weightByEdge">The weight of each edge in the graph.</param>
        /// <param name="distanceByVertex">The distance to each discovered vertex.</param>
        /// <param name="weightComparer">The weight comparer.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TWeightMap">The type of the weight map.</typeparam>
        /// <typeparam name="TDistanceMap">The type of the distance map.</typeparam>
        /// <typeparam name="TWeightComparer">The type of the weight comparer.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>,
        /// or <paramref name="distanceByVertex"/> is <see langword="null"/>,
        /// or <paramref name="weightComparer"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight> =>
            EnumerateEdgesChecked(graph, source, weightByEdge, distanceByVertex, weightComparer);

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            Dictionary<TVertex, TWeight> distanceByVertex = new();
            var weightComparer = Comparer<TWeight>.Default;
            return EnumerateEdgesUnchecked(
                graph, source, weightByEdge, distanceByVertex, weightComparer);
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (weightByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(weightByEdge));

            if (distanceByVertex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(distanceByVertex));

            var weightComparer = Comparer<TWeight>.Default;
            return EnumerateEdgesUnchecked(
                graph, source, weightByEdge, distanceByVertex, weightComparer);
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<Endpoints<TVertex>> EnumerateEdgesUnchecked<
            TGraph, TWeightMap, TDistanceMap, TWeightComparer>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex,
            TWeightComparer weightComparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TWeightMap : IReadOnlyDictionary<Endpoints<TVertex>, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight>
            where TWeightComparer : IComparer<TWeight>
        {
            AdditiveMonoid<TWeight> weightMonoid = default;
            return EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>.EnumerateEdgesUnchecked(
                graph, source, weightByEdge, distanceByVertex, weightMonoid, weightComparer);
        }
    }
}
#endif
