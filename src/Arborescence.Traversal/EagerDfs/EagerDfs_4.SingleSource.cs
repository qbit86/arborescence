namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EagerDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        public void Traverse<TColorMap, THandler>(TGraph graph, TVertex source, TColorMap colorMap, THandler handler)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorMap, handler, s_false);
        }

        public void Traverse<TColorMap, THandler>(TGraph graph, TVertex source, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorMap, handler, terminationCondition ?? s_false);
        }
    }
}
