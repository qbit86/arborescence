namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Traversal;

    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AStarHeuristic.html

    /// <summary>
    /// Implements a heuristic search on a weighted graph for the case where all edge weights are non-negative.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TCost">The type of the weight assigned to each edge.</typeparam>
    /// <typeparam name="TCostComparer">The type of the cost comparer.</typeparam>
    /// <typeparam name="TCostMonoid">The type of the cost monoid.</typeparam>
    public readonly struct EnumerableAStar<TGraph, TVertex, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoid>
        where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TCostComparer : IComparer<TCost>
        where TCostMonoid : IMonoid<TCost>
        where TVertex : notnull
        where TEdge : notnull
    {
        private readonly TCostComparer _costComparer;
        private readonly TCostMonoid _costMonoid;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnumerableAStar{TGraph,TVertex,TEdge,TEdgeEnumerator,TCost,TCostComparer,TCostMonoid}"/> structure.
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

        // https://github.com/boostorg/graph/blob/97f51d81800cd5ed7d55e48a02b18e2aad3bb8e0/include/boost/graph/astar_search.hpp#L176..L232
        // https://github.com/boostorg/graph/issues/233

        /// <summary>
        /// Enumerates the edges of the graph as the algorithm considers them to be a part of a search tree.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="heuristic">A heuristic function to estimate the cost from a given vertex to some goal state.</param>
        /// <param name="weightByEdge">The weight of each edge in the graph.</param>
        /// <param name="costByVertex">The estimated cost to the goal of the path through a given vertex.</param>
        /// <param name="distanceByVertex">The shortest path weight from the source vertex to a given vertex.</param>
        /// <param name="colorByVertex">Indicates whether a given vertex is on the OPEN or CLOSED lists.</param>
        /// <param name="indexByVertex">
        /// Maps each vertex to an integer in the range [0, vertexCount) to use for fast search in priority queue.
        /// </param>
        /// <typeparam name="TCostMap">The type of the cost map.</typeparam>
        /// <typeparam name="TDistanceMap">The type of the distance map.</typeparam>
        /// <typeparam name="TWeightMap">The type of the weight map.</typeparam>
        /// <typeparam name="TColorMap">The type of the color map.</typeparam>
        /// <typeparam name="TIndexMap">The type of the index map.</typeparam>
        /// <returns>An enumerator to enumerate the edges as the algorithm relaxes them.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="heuristic"/> is <see langword="null"/>,
        /// or <paramref name="costByVertex"/> is <see langword="null"/>,
        /// or <paramref name="distanceByVertex"/> is <see langword="null"/>,
        /// or <paramref name="weightByEdge"/> is <see langword="null"/>,
        /// or <paramref name="colorByVertex"/> is <see langword="null"/>,
        /// or <paramref name="indexByVertex"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TEdge> EnumerateRelaxedEdges<
            TCostMap, TDistanceMap, TWeightMap, TColorMap, TIndexMap>(
            TGraph graph,
            TVertex source,
            Func<TVertex, TCost> heuristic,
            TWeightMap weightByEdge,
            TCostMap costByVertex,
            TDistanceMap distanceByVertex,
            TColorMap colorByVertex,
            TIndexMap indexByVertex)
            where TCostMap : IReadOnlyDictionary<TVertex, TCost>, IDictionary<TVertex, TCost>
            where TDistanceMap : IDictionary<TVertex, TCost>
            where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
            where TColorMap : IDictionary<TVertex, Color>
            where TIndexMap : IDictionary<TVertex, int>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (heuristic is null)
                ThrowHelper.ThrowArgumentNullException(nameof(heuristic));

            if (costByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(costByVertex));

            if (distanceByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(distanceByVertex));

            if (weightByEdge is null)
                ThrowHelper.ThrowArgumentNullException(nameof(weightByEdge));

            if (colorByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(colorByVertex));

            if (indexByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByVertex));

            return EnumerateRelaxedEdgesIterator(
                graph, source, heuristic, weightByEdge, costByVertex, distanceByVertex, colorByVertex, indexByVertex);
        }

        internal IEnumerator<TEdge> EnumerateRelaxedEdgesIterator<
            TCostMap, TDistanceMap, TWeightMap, TColorMap, TIndexMap>(
            TGraph graph,
            TVertex source,
            Func<TVertex, TCost> heuristic,
            TWeightMap weightByEdge,
            TCostMap costByVertex,
            TDistanceMap distanceByVertex,
            TColorMap colorByVertex,
            TIndexMap indexByVertex)
            where TCostMap : IReadOnlyDictionary<TVertex, TCost>, IDictionary<TVertex, TCost>
            where TDistanceMap : IDictionary<TVertex, TCost>
            where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
            where TColorMap : IDictionary<TVertex, Color>
            where TIndexMap : IDictionary<TVertex, int>
        {
            distanceByVertex[source] = _costMonoid.Identity;
            SetCost(costByVertex, source, heuristic(source));

            var queue = new MinHeap<TVertex, TCost, TCostMap, TIndexMap, TCostComparer>(
                costByVertex, indexByVertex, _costComparer);
            try
            {
                colorByVertex[source] = Color.Gray;
                queue.Add(source);
                while (queue.TryTake(out TVertex? u))
                {
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    try
                    {
                        while (outEdges.MoveNext())
                        {
                            if (!(outEdges.Current is { } e))
                                continue;

                            if (!graph.TryGetHead(e, out TVertex? v))
                                continue;

                            // examine_edge
                            if (!weightByEdge.TryGetValue(e, out TCost? weight))
                                continue;

                            if (_costComparer.Compare(weight, _costMonoid.Identity) < 0)
                                AStarHelper.ThrowInvalidOperationException_NegativeWeight();

                            bool decreased = Relax(u, v, weight, distanceByVertex, out TCost? relaxedHeadDistance);
                            if (decreased)
                            {
                                TCost vCost = _costMonoid.Combine(relaxedHeadDistance!, heuristic(v));
                                SetCost(costByVertex, v, vCost);
                                yield return e;
                            }

                            Color vColor = GetColorOrDefault(colorByVertex, v);
                            switch (vColor)
                            {
                                case Color.None:
                                case Color.White:
                                    // tree_edge
                                    colorByVertex[v] = Color.Gray;
                                    queue.Add(v);
                                    break;
                                case Color.Gray:
                                    // gray_target
                                    if (decreased)
                                        queue.Update(v);
                                    break;
                                case Color.Black:
                                    // black_target
                                    if (decreased)
                                    {
                                        colorByVertex[v] = Color.Gray;
                                        queue.Add(v);
                                    }

                                    break;
                            }
                        }
                    }
                    finally
                    {
                        outEdges.Dispose();
                    }

                    colorByVertex[u] = Color.Black;
                }
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }

        // https://github.com/boostorg/graph/blob/97f51d81800cd5ed7d55e48a02b18e2aad3bb8e0/include/boost/graph/relax.hpp#L61..L73

        private bool Relax<TDistanceMap>(
            TVertex tail,
            TVertex head,
            TCost weight,
            TDistanceMap distanceByVertex,
            [MaybeNullWhen(false)] out TCost relaxedHeadDistance)
            where TDistanceMap : IDictionary<TVertex, TCost>
        {
            if (!distanceByVertex.TryGetValue(tail, out TCost? tailDistance))
            {
                relaxedHeadDistance = default;
                return false;
            }

            relaxedHeadDistance = _costMonoid.Combine(tailDistance, weight);
            if (distanceByVertex.TryGetValue(head, out TCost? currentHeadDistance) &&
                _costComparer.Compare(relaxedHeadDistance, currentHeadDistance) >= 0)
                return false;

            distanceByVertex[head] = relaxedHeadDistance;
            return true;
        }

        // Ambiguous indexer:
        // TCost this[TVertex] (in interface IDictionary<TVertex,TCost>)
        // TCost this[TVertex] (in interface IReadOnlyDictionary<TVertex,TCost>)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetCost<TCostMap>(TCostMap costByVertex, TVertex vertex, TCost cost)
            where TCostMap : IDictionary<TVertex, TCost> =>
            costByVertex[vertex] = cost;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
