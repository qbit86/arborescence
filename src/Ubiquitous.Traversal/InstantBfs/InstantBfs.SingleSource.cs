namespace Ubiquitous.Traversal
{
    using System;
    using Collections;
    using Internal;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct InstantBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
    {
        public void Traverse<THandler>(
            TGraph graph, TVertex source, TColorMap colorMap, THandler handler)
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            System.Collections.Generic.Queue<TVertex> queue = QueueCache<TVertex>.Acquire();
            try
            {
                var queueAdapter = new QueueAdapter<TVertex>(queue);

                ColorMapPolicy.AddOrUpdate(colorMap, source, Color.Gray);
                handler.OnDiscoverVertex(graph, source);
                queueAdapter.Add(source);

                TraverseCore(graph, queueAdapter, colorMap, handler);
            }
            finally
            {
                QueueCache<TVertex>.Release(queue);
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
