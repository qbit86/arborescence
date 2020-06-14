namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Collections;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy,
        TExploredSetPolicy>
    {
        private IEnumerator<TVertex> EnumerateVerticesCore<TContainer>(
            TGraph graph, TContainer queue, TExploredSet exploredSet)
            where TContainer : IContainer<TVertex>
        {
            Debug.Assert(queue != null, "queue != null");

            while (queue.TryTake(out TVertex u))
            {
                TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    bool vExplored = ExploredSetPolicy.Contains(exploredSet, v);
                    if (!vExplored)
                    {
                        ExploredSetPolicy.Add(exploredSet, v);
                        yield return v;
                        queue.Add(v);
                    }
                }
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
