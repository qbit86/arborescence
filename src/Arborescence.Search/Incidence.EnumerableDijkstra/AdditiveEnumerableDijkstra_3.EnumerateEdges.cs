#if NET7_0_OR_GREATER
namespace Arborescence.Search.Incidence
{
    using System.Collections.Generic;

    public static partial class AdditiveEnumerableDijkstra<TVertex, TEdge, TWeight>
    {
        /// <summary>
        /// Enumerates edges of the graph in an order defined by the Dijkstra algorithm, starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="weightByEdge">The weight of each edge in the graph.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TWeightMap">The type of the weight map.</typeparam>
        /// <returns>An enumerable collection of the edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TWeightMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, TEdge, IEnumerator<TEdge>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge);

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
        /// <returns>An enumerable collection of the edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>,
        /// or <paramref name="distanceByVertex"/> is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TWeightMap, TDistanceMap>(
            TGraph graph, TVertex source, TWeightMap weightByEdge, TDistanceMap distanceByVertex)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TWeightMap : IReadOnlyDictionary<TEdge, TWeight>
            where TDistanceMap : IDictionary<TVertex, TWeight> =>
            AdditiveEnumerableDijkstra<TVertex, TEdge, IEnumerator<TEdge>, TWeight>.EnumerateEdgesChecked(
                graph, source, weightByEdge, distanceByVertex);

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
        /// <returns>An enumerable collection of the edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>,
        /// or <paramref name="distanceByVertex"/> is <see langword="null"/>,
        /// or <paramref name="weightComparer"/> is <see langword="null"/>.
        /// </exception>
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
