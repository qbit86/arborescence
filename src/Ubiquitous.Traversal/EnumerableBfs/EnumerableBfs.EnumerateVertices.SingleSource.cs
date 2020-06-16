namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var queue = new Internal.Queue<TVertex>();

            ExploredSetPolicy.Add(exploredSet, source);
            queue.Add(source);

            return EnumerateVerticesCore(graph, queue, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
