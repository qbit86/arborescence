#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET47_OR_GREATER
namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateEdgesChecked(graph, source, frontier);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateEdgesChecked(graph, source, frontier, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateEdgesChecked(graph, source, frontier, exploredSet);

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            HashSet<TVertex> exploredSet = new();
            return EnumerateEdgesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateEdgesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<
            TGraph, TFrontier, TExploredSet>(TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
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

            return EnumerateEdgesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesIterator<
            TGraph, TFrontier, TExploredSet>(TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            exploredSet.Add(source);
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

                        yield return (current, neighbor);
                        exploredSet.Add(neighbor);
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
#endif
