namespace Arborescence.Search.Specialized
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Traversal;

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
                return Enumerable.Empty<TEdge>().GetEnumerator();

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
                // TODO: Replace with special dummy-aware dictionary.
                var costByVertex = new IndexedDictionary<TCost>(costByVertexFromPool);
                var distanceByVertex = new IndexedDictionary<TCost>(distanceByVertexFromPool);
                var colorByVertex = new IndexedColorDictionary(colorByVertexFromPool);
                var indexByVertex = new IndexedDictionary<int>(indexByVertexFromPool);
                // TODO: Replace with iterating!
                return aStar.EnumerateRelaxedEdges(graph, source, heuristic, weightByEdge,
                    costByVertex, distanceByVertex, colorByVertex, indexByVertex);
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
