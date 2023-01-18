namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableDfs<TVertex, TNeighborEnumerator>
    {
        private static IEnumerator<Endpoints<TVertex>> EnumerateEdgesIterator<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var frontier = new ValueStack<StackFrame>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    frontier.Add(new(source, graph.EnumerateNeighbors(source)));

                    while (frontier.TryTake(out StackFrame stackFrame))
                    {
                        (TVertex current, TNeighborEnumerator neighborEnumerator) = stackFrame;
                        if (!neighborEnumerator.MoveNext())
                        {
                            neighborEnumerator.Dispose();
                            continue;
                        }

                        TVertex neighbor = neighborEnumerator.Current;
                        frontier.Add(stackFrame with { NeighborEnumerator = neighborEnumerator });
                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return new(current, neighbor);
                        exploredSet.Add(neighbor);
                        frontier.Add(new(neighbor, graph.EnumerateNeighbors(neighbor)));
                    }
                }
            }
            finally
            {
                while (frontier.TryTake(out StackFrame stackFrame))
                    stackFrame.Dispose();
                frontier.Dispose();
            }
        }
    }
}
