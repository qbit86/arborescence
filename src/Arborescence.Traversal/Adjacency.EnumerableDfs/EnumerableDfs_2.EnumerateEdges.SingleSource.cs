namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableDfs<TVertex, TNeighborEnumerator>
    {
        private static IEnumerator<Endpoints<TVertex>> EnumerateEdgesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            var frontier = new ValueStack<StackFrame>();
            try
            {
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
            finally
            {
                while (frontier.TryTake(out StackFrame stackFrame))
                    stackFrame.Dispose();
                frontier.Dispose();
            }
        }
    }
}
