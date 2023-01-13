namespace Arborescence.Traversal.Adjacency
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class EnumerableGenericSearch
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked<TVertex, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
                graph, source, frontier, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<
            TVertex, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateVerticesIterator<TVertex, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
                graph, source, frontier, exploredSet);
        }

        private static IEnumerator<TVertex> EnumerateVerticesIterator<
            TVertex, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
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
                TVertexEnumerator neighbors = graph.EnumerateAdjacentVertices(current);
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
