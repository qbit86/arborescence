namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct InstantBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
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

            var queue = new Internal.Queue<TVertex>();
            while (sources.MoveNext())
            {
                TVertex s = sources.Current;
                ColorMapPolicy.AddOrUpdate(colorMap, s, Color.Gray);
                handler.OnDiscoverVertex(graph, s);
                queue.Add(s);
            }

            TraverseCore(graph, queue, colorMap, handler);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
