namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;

    public static partial class EnumerableDfs<TVertex, TEdge, TEdgeEnumerator>
    {
        private static IEnumerable<TEdge> EnumerateEdgesIterator<TGraph, TExploredSet>(
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

                while (stack.TryTake(out TEdgeEnumerator edgeEnumerator))
                {
                    if (!edgeEnumerator.MoveNext())
                    {
                        edgeEnumerator.Dispose();
                        continue;
                    }

                    TEdge edge = edgeEnumerator.Current;
                    stack.Add(edgeEnumerator);

                    if (!graph.TryGetHead(edge, out TVertex? neighbor))
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
                while (stack.TryTake(out TEdgeEnumerator stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
            }
        }
    }
}
