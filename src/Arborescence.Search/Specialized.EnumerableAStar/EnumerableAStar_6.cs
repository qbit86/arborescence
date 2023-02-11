namespace Arborescence.Search.Specialized
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Implements a heuristic search on a weighted graph for the case where all edge weights are non-negative.
    /// </summary>
    /// <remarks>
    /// This implementation assumes vertices to be indices in range [0, vertexCount).
    /// </remarks>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TCost">The type of the weight assigned to each edge.</typeparam>
    /// <typeparam name="TCostComparer">The type of the cost comparer.</typeparam>
    /// <typeparam name="TCostMonoid">The type of the cost monoid.</typeparam>
    public readonly struct EnumerableAStar<TGraph, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoid>
        where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TCostComparer : IComparer<TCost>
        where TCostMonoid : IMonoid<TCost>
        where TEdge : notnull
    {
        private readonly TCostComparer _costComparer;
        private readonly TCostMonoid _costMonoid;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnumerableAStar{TGraph,TEdge,TEdgeEnumerator,TCost,TCostComparer,TCostMonoid}"/> structure.
        /// </summary>
        /// <param name="costComparer">The <typeparamref name="TCostComparer"/> to use when comparing distances and priorities.</param>
        /// <param name="costMonoid">The <typeparamref name="TCostMonoid"/> to use when updating the distance map and the cost map.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="costComparer"/> is <see langword="null"/>,
        /// or <paramref name="costMonoid"/> is <see langword="null"/>.
        /// </exception>
        public EnumerableAStar(TCostComparer costComparer, TCostMonoid costMonoid)
        {
            if (costComparer is null)
                ThrowHelper.ThrowArgumentNullException(nameof(costComparer));

            if (costMonoid is null)
                ThrowHelper.ThrowArgumentNullException(nameof(costMonoid));

            _costComparer = costComparer;
            _costMonoid = costMonoid;
        }

        /// <summary>
        /// Enumerates the edges of the graph as the algorithm considers them to be a part of a search tree.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="heuristic">A heuristic function to estimate the cost from a given vertex to some goal state.</param>
        /// <param name="weightByEdge">The weight of each edge in the graph.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <param name="infinity">The marker for missing values.</param>
        /// <typeparam name="TWeightMap">The type of the weight map.</typeparam>
        /// <returns>An enumerator to enumerate the edges as the algorithm relaxes them.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="heuristic"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        public IEnumerator<TEdge> EnumerateRelaxedEdges<TWeightMap>(
            TGraph graph,
            int source,
            Func<int, TCost> heuristic,
            TWeightMap weightByEdge,
            int vertexCount,
            TCost infinity)
            where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (heuristic is null)
                ThrowHelper.ThrowArgumentNullException(nameof(heuristic));

            if (weightByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(weightByEdge));

            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            return EnumerateRelaxedEdgesIterator(graph, source, heuristic, weightByEdge, vertexCount, infinity);
        }

        private IEnumerator<TEdge> EnumerateRelaxedEdgesIterator<TWeightMap>(
            TGraph graph,
            int source,
            Func<int, TCost> heuristic,
            TWeightMap weightByEdge,
            int vertexCount,
            TCost infinity)
            where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
        {
            if (unchecked((uint)source >= vertexCount))
                yield break;

            var dummy = new DummyEqualityComparer<TCost, TCostComparer>(infinity, _costComparer);
            var aStar = new EnumerableAStar<TGraph, int, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoid>(
                _costComparer, _costMonoid);

            TCost[] costByVertexFromPool = ArrayPool<TCost>.Shared.Rent(vertexCount);
            Fill(costByVertexFromPool, infinity, 0, vertexCount);
            TCost[] distanceByVertexFromPool = ArrayPool<TCost>.Shared.Rent(vertexCount);
            Fill(distanceByVertexFromPool, infinity, 0, vertexCount);
            byte[] colorByVertexFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(colorByVertexFromPool, 0, vertexCount);
            int[] indexByVertexFromPool = ArrayPool<int>.Shared.Rent(vertexCount);
            Fill(indexByVertexFromPool, -1, 0, vertexCount);
            try
            {
                var costByVertex = new IndexedDictionary<TCost, DummyEqualityComparer<TCost, TCostComparer>>(
                    costByVertexFromPool, dummy);
                var distanceByVertex = new IndexedDictionary<TCost, DummyEqualityComparer<TCost, TCostComparer>>(
                    distanceByVertexFromPool, dummy);
                var colorByVertex = new IndexedColorDictionary(colorByVertexFromPool);
                var indexByVertex = new IndexedDictionary<int>(indexByVertexFromPool);
                using IEnumerator<TEdge> edges = aStar.EnumerateRelaxedEdgesIterator(graph, source, heuristic,
                    weightByEdge, costByVertex, distanceByVertex, colorByVertex, indexByVertex);
                while (edges.MoveNext())
                    yield return edges.Current;
            }
            finally
            {
                ArrayPool<int>.Shared.Return(indexByVertexFromPool);
                ArrayPool<byte>.Shared.Return(colorByVertexFromPool);
                ArrayPool<TCost>.Shared.Return(distanceByVertexFromPool);
                ArrayPool<TCost>.Shared.Return(costByVertexFromPool);
            }
        }

        private static void Fill<T>(T[] array, T value, int startIndex, int count)
        {
            Debug.Assert(startIndex >= 0, "startIndex >= 0");
            Debug.Assert(startIndex <= array.Length, "startIndex <= array.Length");
            Debug.Assert(count >= 0, "count >= 0");
            int end = startIndex + count;
            Debug.Assert(end <= array.Length, "end <= array.Length");

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            Array.Fill(array, value, startIndex, count);
#else
            for (int i = startIndex; i < end; ++i)
                array[i] = value;
#endif
        }
    }
}
