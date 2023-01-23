namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;

    public static partial class EnumerableDfs<TVertex, TEdge, TEdgeEnumerator>
    {
        private static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            yield return source;
            var frontier = new ValueStack<TEdgeEnumerator>();
            try
            {
                frontier.Add(graph.EnumerateOutEdges(source));

                while (frontier.TryTake(out TEdgeEnumerator edgeEnumerator))
                {
                    if (!edgeEnumerator.MoveNext())
                    {
                        edgeEnumerator.Dispose();
                        continue;
                    }

                    TEdge edge = edgeEnumerator.Current;
                    frontier.Add(edgeEnumerator);

                    if (!graph.TryGetHead(edge, out TVertex? neighbor))
                        continue;

                    if (!exploredSet.Add(neighbor))
                        continue;
                    yield return neighbor;
                    frontier.Add(graph.EnumerateOutEdges(neighbor));
                }
            }
            finally
            {
                while (frontier.TryTake(out TEdgeEnumerator edgeEnumerator))
                    edgeEnumerator.Dispose();
                frontier.Dispose();
            }
        }
    }
}
