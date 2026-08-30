namespace Arborescence.Traversal.Internal.Adjacency
{
    using System;
    using System.Collections.Generic;

    internal static class EnumerableDfs<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            yield return source;
            var stack = new ValueStack<TNeighborEnumerator>();
            try
            {
                stack.Add(graph.EnumerateOutNeighbors(source));

                while (stack.TryTake(out var neighborEnumerator))
                {
                    if (!neighborEnumerator.MoveNext())
                    {
                        neighborEnumerator.Dispose();
                        continue;
                    }

                    var neighbor = neighborEnumerator.Current;
                    stack.Add(neighborEnumerator);
                    if (!exploredSet.Add(neighbor))
                        continue;
                    yield return neighbor;
                    stack.Add(graph.EnumerateOutNeighbors(neighbor));
                }
            }
            finally
            {
                while (stack.TryTake(out var stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
            }
        }

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var sourceEnumerator = sources.GetEnumerator();
            var stack = new ValueStack<TNeighborEnumerator>();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    yield return source;
                    stack.Add(graph.EnumerateOutNeighbors(source));

                    while (stack.TryTake(out var neighborEnumerator))
                    {
                        if (!neighborEnumerator.MoveNext())
                        {
                            neighborEnumerator.Dispose();
                            continue;
                        }

                        var neighbor = neighborEnumerator.Current;
                        stack.Add(neighborEnumerator);
                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        stack.Add(graph.EnumerateOutNeighbors(neighbor));
                    }
                }
            }
            finally
            {
                while (stack.TryTake(out var stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
                sourceEnumerator.Dispose();
            }
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            var stack = new ValueStack<StackFrame>();
            try
            {
                stack.Add(new(source, graph.EnumerateOutNeighbors(source)));

                while (stack.TryTake(out var stackFrame))
                {
                    var (current, neighborEnumerator) = stackFrame;
                    if (!neighborEnumerator.MoveNext())
                    {
                        neighborEnumerator.Dispose();
                        continue;
                    }

                    var neighbor = neighborEnumerator.Current;
                    stack.Add(stackFrame with { NeighborEnumerator = neighborEnumerator });
                    if (exploredSet.Contains(neighbor))
                        continue;

                    yield return new(current, neighbor);
                    exploredSet.Add(neighbor);
                    stack.Add(new(neighbor, graph.EnumerateOutNeighbors(neighbor)));
                }
            }
            finally
            {
                while (stack.TryTake(out var stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
            }
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<
            TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var sourceEnumerator = sources.GetEnumerator();
            var stack = new ValueStack<StackFrame>();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    stack.Add(new(source, graph.EnumerateOutNeighbors(source)));

                    while (stack.TryTake(out var stackFrame))
                    {
                        var (current, neighborEnumerator) = stackFrame;
                        if (!neighborEnumerator.MoveNext())
                        {
                            neighborEnumerator.Dispose();
                            continue;
                        }

                        var neighbor = neighborEnumerator.Current;
                        stack.Add(stackFrame with { NeighborEnumerator = neighborEnumerator });
                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return new(current, neighbor);
                        exploredSet.Add(neighbor);
                        stack.Add(new(neighbor, graph.EnumerateOutNeighbors(neighbor)));
                    }
                }
            }
            finally
            {
                while (stack.TryTake(out var stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
                sourceEnumerator.Dispose();
            }
        }

        private readonly record struct StackFrame(TVertex Current, TNeighborEnumerator NeighborEnumerator) :
            IDisposable
        {
            public void Dispose() => NeighborEnumerator.Dispose();
        }
    }
}
