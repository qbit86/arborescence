namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using Collections;
    using Internal;

    public readonly partial struct InstantBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy,
        TColorMapPolicy>
    {
        public void Traverse<TVertexEnumerator, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorMap, THandler handler)
            where TVertexEnumerator : IEnumerator<TVertex>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Queue<TVertex> queue = QueueCache<TVertex>.Acquire();
            try
            {
                var queueAdapter = new QueueAdapter<TVertex>(queue);

                while (sources.MoveNext())
                {
                    TVertex s = sources.Current;
                    ColorMapPolicy.AddOrUpdate(colorMap, s, Color.Gray);
                    handler.OnDiscoverVertex(graph, s);
                    queueAdapter.Add(s);
                }

                TraverseCore(graph, queueAdapter, colorMap, handler);
            }
            finally
            {
                QueueCache<TVertex>.Release(queue);
            }
        }
    }
}
