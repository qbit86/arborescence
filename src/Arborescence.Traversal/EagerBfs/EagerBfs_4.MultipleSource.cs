namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EagerBfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        public void Traverse<TVertexEnumerator, TColorMap, THandler>(
            TGraph graph, TVertexEnumerator sources, TColorMap colorMap, THandler handler)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var queue = new Internal.Queue<TVertex>();
            while (sources.MoveNext())
            {
                TVertex s = sources.Current;
                colorMap[s] = Color.Gray;
                handler.OnDiscoverVertex(graph, s);
                queue.Add(s);
            }

            TraverseCore(graph, queue, colorMap, handler);
        }
    }
}
