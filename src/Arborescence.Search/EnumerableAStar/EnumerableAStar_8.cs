namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;

    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AStarHeuristic.html

    public readonly struct EnumerableAStar<TGraph, TEdge, TEdgeEnumerator, TCost, TWeightMap, TCostMap,
        TCostComparer, TCostMonoid>
        where TGraph : IIncidenceGraph<int, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TCostComparer : IComparer<TCost>
        where TCostMonoid : IMonoid<TCost>
        where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
        where TCostMap : IDictionary<int, TCost>
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
    }
}
