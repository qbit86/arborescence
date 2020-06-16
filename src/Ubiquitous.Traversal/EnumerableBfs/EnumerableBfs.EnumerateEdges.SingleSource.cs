namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var queue = new Internal.Queue<TVertex>();

            ExploredSetPolicy.Add(exploredSet, source);
            queue.Add(source);

            return EnumerateEdgesCore(graph, queue, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
