namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;
#endif

    public static partial class EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
    {
        internal static IEnumerable<TEdge> EnumerateEdgesIterator<
            TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                if (!exploredSet.Add(source))
                    continue;
                frontier.AddOrThrow(source);
            }

            while (frontier.TryTake(out TVertex? current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                TEdgeEnumerator outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        TEdge edge = outEdges.Current;
                        if (!graph.TryGetHead(edge, out TVertex? neighbor))
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
