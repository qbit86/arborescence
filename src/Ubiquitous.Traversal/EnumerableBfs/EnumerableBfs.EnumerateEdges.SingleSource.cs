namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;
    using Collections;
    using Internal;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            System.Collections.Generic.Queue<TVertex> queue = QueueCache<TVertex>.Acquire();
            var queueAdapter = new QueueAdapter<TVertex>(queue);

            ExploredSetPolicy.Add(exploredSet, source);
            queueAdapter.Add(source);

            return EnumerateEdgesCore(graph, queueAdapter, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
