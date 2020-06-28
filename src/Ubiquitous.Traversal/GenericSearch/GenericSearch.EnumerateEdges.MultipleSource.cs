namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe, TExploredSet,
        TGraphPolicy, TFringePolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TFringe fringe, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                ExploredSetPolicy.Add(exploredSet, source);
                FringePolicy.Add(fringe, source);
            }

            return EnumerateEdgesCore(graph, fringe, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
