namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;
    using Traversal;

    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AStarHeuristic.html

    public readonly struct EnumerableAStar<TGraph, TVertex, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoid>
        where TGraph : IIncidenceGraph<int, TEdge, TEdgeEnumerator>
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
            where TCostMap : IDictionary<TVertex, TCost>
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

            throw new NotImplementedException();
        }
    }
}
