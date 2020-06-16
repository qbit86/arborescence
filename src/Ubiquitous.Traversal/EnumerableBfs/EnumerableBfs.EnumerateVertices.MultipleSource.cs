namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            var queue = new Internal.Queue<TVertex>();

            while (sources.MoveNext())
            {
                TVertex s = sources.Current;
                ExploredSetPolicy.Add(exploredSet, s);
                queue.Add(s);
            }

            return EnumerateVerticesCore(graph, queue, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
