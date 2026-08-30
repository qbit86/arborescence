namespace Arborescence.Traversal.Internal.Incidence
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;
#endif

    internal static class EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
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
                var outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        if (!graph.TryGetHead(outEdges.Current, out var neighbor))
                            continue;
                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        frontier.AddOrThrow(neighbor);
                    }
                }
                finally
                {
                    outEdges.Dispose();
                }
            }
        }

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<
            TGraph, TSourceCollection, TFrontier, TExploredSet>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
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
                var outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        if (!graph.TryGetHead(outEdges.Current, out var neighbor))
                            continue;
                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        frontier.AddOrThrow(neighbor);
                    }
                }
                finally
                {
                    outEdges.Dispose();
                }
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesIterator<
            TGraph, TFrontier, TExploredSet>(TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
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
                var outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        var edge = outEdges.Current;
                        if (!graph.TryGetHead(edge, out var neighbor))
                            continue;
                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return edge;
                        exploredSet.Add(neighbor);
                        frontier.AddOrThrow(neighbor);
                    }
                }
                finally
                {
                    outEdges.Dispose();
                }
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesIterator<
            TGraph, TSourceCollection, TFrontier, TExploredSet>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
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
                var outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        var edge = outEdges.Current;
                        if (!graph.TryGetHead(edge, out var neighbor))
                            continue;
                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return edge;
                        exploredSet.Add(neighbor);
                        frontier.AddOrThrow(neighbor);
                    }
                }
                finally
                {
                    outEdges.Dispose();
                }
            }
        }
    }
}
