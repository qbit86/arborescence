namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Internal;
    using Traversal;

    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AStarHeuristic.html

    public readonly struct EnumerableAStar<TGraph, TVertex, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoid>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TCostComparer : IComparer<TCost>
        where TCostMonoid : IMonoid<TCost>
    {
        private readonly TCostComparer _costComparer;
        private readonly TCostMonoid _costMonoid;

        public EnumerableAStar(TCostComparer costComparer, TCostMonoid costMonoid)
        {
            if (costComparer == null)
                throw new ArgumentNullException(nameof(costComparer));

            if (costMonoid == null)
                throw new ArgumentNullException(nameof(costMonoid));

            _costComparer = costComparer;
            _costMonoid = costMonoid;
        }

        // https://github.com/boostorg/graph/blob/97f51d81800cd5ed7d55e48a02b18e2aad3bb8e0/include/boost/graph/astar_search.hpp#L176..L232

        public IEnumerator<TEdge> EnumerateRelaxedEdges<
            TPredecessorMap, TCostMap, TDistanceMap, TWeightMap, TColorMap, TIndexMap>(
            TGraph graph,
            TVertex source,
            Func<TVertex, TCost> heuristic,
            TPredecessorMap predecessorByVertex,
            TCostMap costByVertex,
            TDistanceMap distanceByVertex,
            TWeightMap weightByEdge,
            TColorMap colorByVertex,
            TIndexMap indexByVertex)
            where TPredecessorMap : IDictionary<TVertex, TVertex>
            where TCostMap : IReadOnlyDictionary<TVertex, TCost>, IDictionary<TVertex, TCost>
            where TDistanceMap : IDictionary<TVertex, TCost>
            where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
            where TColorMap : IDictionary<TVertex, Color>
            where TIndexMap : IDictionary<TVertex, int>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (heuristic is null)
                throw new ArgumentNullException(nameof(heuristic));

            if (predecessorByVertex == null)
                throw new ArgumentNullException(nameof(predecessorByVertex));

            if (costByVertex == null)
                throw new ArgumentNullException(nameof(costByVertex));

            if (distanceByVertex == null)
                throw new ArgumentNullException(nameof(distanceByVertex));

            if (weightByEdge == null)
                throw new ArgumentNullException(nameof(weightByEdge));

            if (colorByVertex == null)
                throw new ArgumentNullException(nameof(colorByVertex));

            if (indexByVertex == null)
                throw new ArgumentNullException(nameof(indexByVertex));

            distanceByVertex[source] = _costMonoid.Identity;
            SetCost(costByVertex, source, heuristic(source));

            var queue = new MinHeap<TVertex, TCost, TCostMap, TIndexMap, TCostComparer>(
                costByVertex, indexByVertex, _costComparer);
            try
            {
                colorByVertex[source] = Color.Gray;
                queue.Add(source);
                while (queue.TryTake(out TVertex u))
                {
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out TVertex v))
                            continue;

                        // examine_edge
                        if (!weightByEdge.TryGetValue(e, out TCost weight))
                            continue;

                        if (_costComparer.Compare(weight, _costMonoid.Identity) < 0)
                            AStarHelper.ThrowInvalidOperationException_NegativeWeight();

                        bool decreased = Relax(u, v, weight, predecessorByVertex, distanceByVertex);

                        Color vColor = GetColorOrDefault(colorByVertex, v);
                        switch (vColor)
                        {
                            case Color.None:
                            case Color.White:
                                // tree_edge
                                if (decreased)
                                {
                                    yield return e;
                                    throw new NotImplementedException();
                                }

                                colorByVertex[v] = Color.Gray;
                                queue.Add(v);
                                break;
                            case Color.Gray:
                                // gray_target
                                if (decreased)
                                    throw new NotImplementedException();

                                break;
                            case Color.Black:
                                // black_target
                                if (decreased)
                                {
                                    yield return e;
                                    throw new NotImplementedException();
                                }

                                break;
                        }
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

        private bool Relax<TPredecessorMap, TDistanceMap>(
            TVertex tail,
            TVertex head,
            TCost weight,
            TPredecessorMap predecessorByVertex,
            TDistanceMap distanceByVertex)
            where TPredecessorMap : IDictionary<TVertex, TVertex>
            where TDistanceMap : IDictionary<TVertex, TCost>
        {
            throw new NotImplementedException();
        }

        // Ambiguous indexer:
        // TCost this[TVertex] (in interface IDictionary<TVertex,TCost>)
        // TCost this[TVertex] (in interface IReadOnlyDictionary<TVertex,TCost>)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetCost<TCostMap>(TCostMap costByVertex, TVertex vertex, TCost cost)
            where TCostMap : IDictionary<TVertex, TCost>
        {
            costByVertex[vertex] = cost;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
