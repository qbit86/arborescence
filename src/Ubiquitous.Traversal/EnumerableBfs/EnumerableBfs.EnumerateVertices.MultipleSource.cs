namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;
    using Collections;
    using Internal;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            System.Collections.Generic.Queue<TVertex> queue = QueueCache<TVertex>.Acquire();
            var queueAdapter = new QueueAdapter<TVertex>(queue);

            while (sources.MoveNext())
            {
                TVertex s = sources.Current;
                ExploredSetPolicy.Add(exploredSet, s);
                queueAdapter.Add(s);
            }

            return EnumerateVerticesCore(graph, queueAdapter, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
