namespace Arborescence.Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_76_0/libs/graph/doc/AStarHeuristic.html

    public readonly struct EnumerableAStar<TGraph, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoid>
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

        public IEnumerator<TEdge> EnumerateRelaxedEdges<TWeightMap>(
            TGraph graph,
            int source,
            Func<int, TCost> heuristic,
            TWeightMap weightByEdge,
            int vertexCount,
            TCost infinity)
            where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (heuristic is null)
                throw new ArgumentNullException(nameof(heuristic));

            if (weightByEdge == null)
                throw new ArgumentNullException(nameof(weightByEdge));

            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (unchecked((uint)source >= vertexCount))
                yield break;

            throw new NotImplementedException();
        }

        private static void Fill<T>(T[] array, T value, int startIndex, int count)
        {
            Debug.Assert(array != null, "array != null");
            Debug.Assert(startIndex >= 0, "startIndex >= 0");
            Debug.Assert(startIndex <= array.Length, "startIndex <= array.Length");
            Debug.Assert(count >= 0, "count >= 0");
            int end = startIndex + count;
            Debug.Assert(end <= array.Length, "end <= array.Length");

#if NETSTANDARD1_3 || NETSTANDARD2_0 || NET461
            for (int i = startIndex; i < end; ++i)
                array[i] = value;
#else
            Array.Fill(array, value, startIndex, count);
#endif
        }
    }
}
