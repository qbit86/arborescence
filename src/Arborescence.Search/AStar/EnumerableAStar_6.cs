namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;

    public readonly struct EnumerableAStar<TGraph, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoidPolicy>
        where TGraph : IIncidenceGraph<int, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TCostComparer : IComparer<TCost>
        where TCostMonoidPolicy : IMonoidPolicy<TCost>
    {
        private readonly TCostComparer _costComparer;
        private readonly TCostMonoidPolicy _costMonoidPolicy;

        public EnumerableAStar(TCostComparer costComparer, TCostMonoidPolicy costMonoidPolicy)
        {
            if (costComparer == null)
                throw new ArgumentNullException(nameof(costComparer));

            if (costMonoidPolicy == null)
                throw new ArgumentNullException(nameof(costMonoidPolicy));

            _costComparer = costComparer;
            _costMonoidPolicy = costMonoidPolicy;
        }
    }
}
