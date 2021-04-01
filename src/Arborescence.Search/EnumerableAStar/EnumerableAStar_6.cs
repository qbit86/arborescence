namespace Arborescence.Search
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;

#endif

    // https://boost.org/doc/libs/1_75_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_75_0/libs/graph/doc/AStarHeuristic.html

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

        public IEnumerator<TEdge> EnumerateRelaxedEdges<TWeightMap>(
            TGraph graph, int source, Func<int, TCost> heuristic, int vertexCount,
            TWeightMap weightMap)
            where TWeightMap : IReadOnlyDictionary<TEdge, TCost>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (heuristic is null)
                throw new ArgumentNullException(nameof(heuristic));

            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (weightMap == null)
                throw new ArgumentNullException(nameof(weightMap));

            if (unchecked((uint)source >= vertexCount))
                yield break;

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var queue = new Queue<int>();
            try
            {
                MapHelpers.AddOrUpdate(colorMap, source, 1);
                queue.Enqueue(source);
                while (queue.Count > 0)
                {
                    int u = queue.Dequeue();
#if DEBUG
                    Debug.Assert(MapHelpers.ContainsKey(colorMap, u));
#endif
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out int v))
                            continue;

                        if (MapHelpers.ContainsKey(colorMap, v))
                            continue;

                        yield return e;
                        MapHelpers.AddOrUpdate(colorMap, v, 1);
                        queue.Enqueue(v);
                    }

                    MapHelpers.AddOrUpdate(colorMap, u, 2);
                }
            }
            finally
            {
                queue.Clear();
                ArrayPool<byte>.Shared.Return(colorMap);
            }
        }
    }
}
