namespace Ubiquitous.Traversal
{
    using System;
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

            var queue = new Queue<TVertex>();
            ColorMapPolicy.AddOrUpdate(colorMap, source, Color.Gray);
            handler.OnDiscoverVertex(graph, source);
            queue.Add(source);

            TraverseCore(graph, queue, colorMap, handler);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
