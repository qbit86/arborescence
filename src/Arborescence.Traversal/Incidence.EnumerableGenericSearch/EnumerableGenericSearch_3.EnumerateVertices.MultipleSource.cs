namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;
#endif

    public static partial class EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
    {
        internal static IEnumerator<TVertex> EnumerateVerticesIterator<
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
                yield return source;
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
                        if (!graph.TryGetHead(outEdges.Current, out TVertex? neighbor))
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
    }
}
