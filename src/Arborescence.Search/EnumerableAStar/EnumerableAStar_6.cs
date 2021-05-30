namespace Arborescence.Search
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Internal;

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

            TCost[] priorityByElementData = ArrayPool<TCost>.Shared.Rent(vertexCount);
            Array.Clear(priorityByElementData, 0, priorityByElementData.Length);
            var priorityByElement = new IndexedDictionary<TCost>(priorityByElementData);
            int[] indexInHeapByElementData = ArrayPool<int>.Shared.Rent(vertexCount);
            Fill(indexInHeapByElementData, -1, 0, indexInHeapByElementData.Length);
            var indexInHeapByElement = new IndexedDictionary<int>(indexInHeapByElementData);
            var minHeap = new MinHeap<int, TCost, IndexedDictionary<TCost>, IndexedDictionary<int>, Comparer<TCost>>(
                priorityByElement, indexInHeapByElement, Comparer<TCost>.Default);
            byte[] colorMapData = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(colorMapData, 0, colorMapData.Length);
            var queue = new Queue<int>();
            try
            {
                MapHelpers.AddOrUpdate(colorMapData, source, Colors.Gray);
                queue.Enqueue(source);
                while (queue.Count > 0)
                {
                    int u = queue.Dequeue();
#if DEBUG
                    Debug.Assert(MapHelpers.ContainsKey(colorMapData, u));
#endif
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out int v))
                            continue;

                        if (MapHelpers.ContainsKey(colorMapData, v))
                            continue;

                        yield return e;
                        MapHelpers.AddOrUpdate(colorMapData, v, Colors.Gray);
                        queue.Enqueue(v);
                    }

                    MapHelpers.AddOrUpdate(colorMapData, u, Colors.Black);
                }
            }
            finally
            {
                queue.Clear();
                ArrayPool<int>.Shared.Return(indexInHeapByElementData);
                ArrayPool<TCost>.Shared.Return(priorityByElementData);
                ArrayPool<byte>.Shared.Return(colorMapData);
            }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte GetColorOrDefault(IndexedDictionary<byte> colorMap, int vertex) =>
            colorMap.TryGetValue(vertex, out byte result) ? result : default;
    }
}
