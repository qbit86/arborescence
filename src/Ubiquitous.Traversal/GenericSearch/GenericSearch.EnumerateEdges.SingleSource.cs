namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
        TGraphPolicy, TFringePolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges(
            TGraph graph, TVertex source, TFringe fringe, TExploredSet exploredSet)
        {
            ExploredSetPolicy.Add(exploredSet, source);
            FringePolicy.Add(fringe, source);

            return EnumerateEdgesCore(graph, fringe, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
