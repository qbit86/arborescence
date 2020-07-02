namespace Arborescence.Traversal
{
    using System;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    // ReSharper disable UnusedTypeParameter
    public readonly partial struct RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        private static readonly Func<TGraph, TVertex, bool> s_false = (g, v) => false;

        public void Traverse<THandler>(TGraph graph, TVertex source, TColorMap colorMap, THandler handler)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorMap, handler, s_false);
        }

        public void Traverse<THandler>(TGraph graph, TVertex source, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorMap, handler, terminationCondition ?? s_false);
        }
    }
    // ReSharper restore UnusedTypeParameter
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
