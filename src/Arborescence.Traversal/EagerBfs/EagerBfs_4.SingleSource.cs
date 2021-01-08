namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EagerBfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        public void Traverse<TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorMap, THandler handler)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var queue = new Internal.Queue<TVertex>();
            colorMap[source] = Color.Gray;
            handler.OnDiscoverVertex(graph, source);
            queue.Add(source);

            TraverseCore(graph, queue, colorMap, handler);
        }
    }
}
