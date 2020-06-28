﻿namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe,
        TExploredSet, TGraphPolicy, TFringePolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices(
            TGraph graph, TVertex source, TFringe fringe, TExploredSet exploredSet)
        {
            ExploredSetPolicy.Add(exploredSet, source);
            yield return source;
            FringePolicy.Add(fringe, source);

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

                    ExploredSetPolicy.Add(exploredSet, v);
                    yield return v;
                    FringePolicy.Add(fringe, v);
                }
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
