namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateVerticesChecked(graph, source, frontier);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateVerticesChecked(graph, source, frontier, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked(graph, source, frontier, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            HashSet<TVertex> exploredSet = new();
            return EnumerateVerticesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            if (comparer is null)
                ThrowHelper.ThrowArgumentNullException(nameof(comparer));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateVerticesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesIterator<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
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
