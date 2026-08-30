namespace Arborescence.Traversal.Internal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;
#endif

    internal static class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            yield return source;
            frontier.AddOrThrow(source);

            while (frontier.TryTake(out var current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                var neighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        var neighbor = neighbors.Current;
                        if (!exploredSet.Add(neighbor))
                            continue;
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

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<
            TGraph, TSourceCollection, TFrontier, TExploredSet>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var sourceEnumerator = sources.GetEnumerator();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    yield return source;
                    frontier.AddOrThrow(source);
                }
            }
            finally
            {
                sourceEnumerator.Dispose();
            }

            while (frontier.TryTake(out var current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                var neighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        var neighbor = neighbors.Current;
                        if (!exploredSet.Add(neighbor))
                            continue;
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

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            frontier.AddOrThrow(source);

            while (frontier.TryTake(out var current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                var neighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        var neighbor = neighbors.Current;
                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return new(current, neighbor);
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

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<
            TGraph, TSourceCollection, TFrontier, TExploredSet>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var sourceEnumerator = sources.GetEnumerator();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    frontier.AddOrThrow(source);
                }
            }
            finally
            {
                sourceEnumerator.Dispose();
            }

            while (frontier.TryTake(out var current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                var neighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        var neighbor = neighbors.Current;
                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return new(current, neighbor);
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
