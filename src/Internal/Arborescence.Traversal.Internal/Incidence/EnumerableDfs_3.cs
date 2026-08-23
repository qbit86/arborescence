namespace Arborescence.Traversal.Internal.Incidence
{
    using System.Collections.Generic;

    internal static class EnumerableDfs<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            yield return source;
            var stack = new ValueStack<TEdgeEnumerator>();
            try
            {
                stack.Add(graph.EnumerateOutEdges(source));

                while (stack.TryTake(out var edgeEnumerator))
                {
                    if (!edgeEnumerator.MoveNext())
                    {
                        edgeEnumerator.Dispose();
                        continue;
                    }

                    var edge = edgeEnumerator.Current;
                    stack.Add(edgeEnumerator);

                    if (!graph.TryGetHead(edge, out var neighbor))
                        continue;

                    if (!exploredSet.Add(neighbor))
                        continue;
                    yield return neighbor;
                    stack.Add(graph.EnumerateOutEdges(neighbor));
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
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var sourceEnumerator = sources.GetEnumerator();
            var stack = new ValueStack<TEdgeEnumerator>();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    yield return source;
                    stack.Add(graph.EnumerateOutEdges(source));

                    while (stack.TryTake(out var edgeEnumerator))
                    {
                        if (!edgeEnumerator.MoveNext())
                        {
                            edgeEnumerator.Dispose();
                            continue;
                        }

                        var edge = edgeEnumerator.Current;
                        stack.Add(edgeEnumerator);

                        if (!graph.TryGetHead(edge, out var neighbor))
                            continue;

                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        stack.Add(graph.EnumerateOutEdges(neighbor));
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

        internal static IEnumerable<TEdge> EnumerateEdgesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            var stack = new ValueStack<TEdgeEnumerator>();
            try
            {
                stack.Add(graph.EnumerateOutEdges(source));

                while (stack.TryTake(out var edgeEnumerator))
                {
                    if (!edgeEnumerator.MoveNext())
                    {
                        edgeEnumerator.Dispose();
                        continue;
                    }

                    var edge = edgeEnumerator.Current;
                    stack.Add(edgeEnumerator);

                    if (!graph.TryGetHead(edge, out var neighbor))
                        continue;

                    if (exploredSet.Contains(neighbor))
                        continue;

                    yield return edge;
                    exploredSet.Add(neighbor);
                    stack.Add(graph.EnumerateOutEdges(neighbor));
                }
            }
            finally
            {
                while (stack.TryTake(out var stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesIterator<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var sourceEnumerator = sources.GetEnumerator();
            var stack = new ValueStack<TEdgeEnumerator>();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    stack.Add(graph.EnumerateOutEdges(source));

                    while (stack.TryTake(out var edgeEnumerator))
                    {
                        if (!edgeEnumerator.MoveNext())
                        {
                            edgeEnumerator.Dispose();
                            continue;
                        }

                        var edge = edgeEnumerator.Current;
                        stack.Add(edgeEnumerator);

                        if (!graph.TryGetHead(edge, out var neighbor))
                            continue;

                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return edge;
                        exploredSet.Add(neighbor);
                        stack.Add(graph.EnumerateOutEdges(neighbor));
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
    }
}
