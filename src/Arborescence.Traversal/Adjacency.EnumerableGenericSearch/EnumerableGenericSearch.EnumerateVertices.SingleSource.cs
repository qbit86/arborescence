namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableGenericSearch
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, TNeighborEnumerator>.EnumerateVerticesChecked(graph, source, frontier);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, TNeighborEnumerator>.EnumerateVerticesChecked(
                graph, source, frontier, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableGenericSearch<TVertex, TNeighborEnumerator>.EnumerateVerticesChecked(
                graph, source, frontier, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesIterator<
            TVertex, TNeighborEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            exploredSet.Add(source);
            yield return source;
            frontier.AddOrThrow(source);

            while (frontier.TryTake(out TVertex? current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                TNeighborEnumerator neighbors = graph.EnumerateAdjacentVertices(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        TVertex neighbor = neighbors.Current;
                        if (exploredSet.Contains(neighbor))
                            continue;

                        exploredSet.Add(neighbor);
                        yield return neighbor;
                        frontier.AddOrThrow(neighbor);
                    }
                }
                finally
                {
                    neighbors.Dispose();
                }
            }
        }
    }
}
