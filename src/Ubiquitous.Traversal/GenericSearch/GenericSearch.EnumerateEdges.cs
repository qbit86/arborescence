namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
        TGraphPolicy, TFringePolicy, TExploredSetPolicy>
    {
        private IEnumerator<TEdge> EnumerateEdgesCore(TGraph graph, TFringe fringe, TExploredSet exploredSet)
        {
            while (FringePolicy.TryTake(fringe, out TVertex u))
            {
                TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    if (ExploredSetPolicy.Contains(exploredSet, v))
                        continue;

                    yield return e;
                    ExploredSetPolicy.Add(exploredSet, v);
                    FringePolicy.Add(fringe, v);
                }
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
