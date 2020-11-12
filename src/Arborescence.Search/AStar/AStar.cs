namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;

    // https://boost.org/doc/libs/1_74_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_74_0/libs/graph/doc/AStarHeuristic.html

    public readonly struct AStar<TGraph, TEdge, TEdgeEnumerator, TCost, TWeightMap, TCostMap,
        TCostComparer, TCostMonoidPolicy, TWeightMapPolicy, TCostMapPolicy>
        where TGraph : IOutEdgesConcept<int, TEdgeEnumerator>, IHeadConcept<int, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TCostComparer : IComparer<TCost>
        where TCostMonoidPolicy : IMonoidPolicy<TCost>
        where TWeightMapPolicy : IReadOnlyMapPolicy<TWeightMap, TEdge, TCost>
        where TCostMapPolicy : IMapPolicy<TCostMap, int, TCost>
    {
        private readonly TCostComparer _costComparer;
        private readonly TCostMonoidPolicy _costMonoidPolicy;
        private readonly TWeightMapPolicy _weightMapPolicy;
        private readonly TCostMapPolicy _costMapPolicy;

        public AStar(TCostComparer costComparer, TCostMonoidPolicy costMonoidPolicy,
            TWeightMapPolicy weightMapPolicy, TCostMapPolicy costMapPolicy)
        {
            if (costComparer is null)
                throw new ArgumentNullException(nameof(costComparer));

            if (costMonoidPolicy is null)
                throw new ArgumentNullException(nameof(costMonoidPolicy));

            if (weightMapPolicy is null)
                throw new ArgumentNullException(nameof(weightMapPolicy));

            if (costMapPolicy is null)
                throw new ArgumentNullException(nameof(costMapPolicy));

            _costComparer = costComparer;
            _costMonoidPolicy = costMonoidPolicy;
            _weightMapPolicy = weightMapPolicy;
            _costMapPolicy = costMapPolicy;
        }
    }
}
