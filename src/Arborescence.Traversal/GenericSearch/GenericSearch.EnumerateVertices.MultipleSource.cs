﻿namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe,
        TExploredSet, TGraphPolicy, TFringePolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TFringe fringe, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                FringePolicy.Add(fringe, source);
            }

            while (FringePolicy.TryTake(fringe, out TVertex u))
            {
                if (ExploredSetPolicy.Contains(exploredSet, u))
                    continue;

                ExploredSetPolicy.Add(exploredSet, u);
                yield return u;

                TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    FringePolicy.Add(fringe, v);
                }
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}