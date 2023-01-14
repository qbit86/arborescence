#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET47_OR_GREATER
namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
    {
        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesIterator<
            TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                exploredSet.Add(source);
                frontier.AddOrThrow(source);
            }

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
