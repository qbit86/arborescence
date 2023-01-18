namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableDfs<TVertex, TNeighborEnumerator>
    {
        private static IEnumerator<TVertex> EnumerateVerticesIterator<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var frontier = new ValueStack<TNeighborEnumerator>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    yield return source;
                    frontier.Add(graph.EnumerateNeighbors(source));

                    while (frontier.TryTake(out TNeighborEnumerator neighborEnumerator))
                    {
                        if (!neighborEnumerator.MoveNext())
                        {
                            neighborEnumerator.Dispose();
                            continue;
                        }

                        TVertex neighbor = neighborEnumerator.Current;
                        frontier.Add(neighborEnumerator);
                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        frontier.Add(graph.EnumerateNeighbors(neighbor));
                    }
                }
            }
            finally
            {
                while (frontier.TryTake(out TNeighborEnumerator neighborEnumerator))
                    neighborEnumerator.Dispose();
                frontier.Dispose();
            }
        }
    }
}
