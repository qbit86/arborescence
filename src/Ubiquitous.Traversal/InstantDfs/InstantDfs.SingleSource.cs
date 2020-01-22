namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public readonly partial struct InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
    {
        private static readonly Func<TGraph, TVertex, bool> s_false = (g, v) => false;

        public void Traverse<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler)
            where THandler : IDfsHandler<TGraph, TVertex>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);
        }

        public void Traverse<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, terminationCondition ?? s_false);
        }

        private void TraverseCore<THandler>(TGraph graph, TVertex u, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex>
        {
            Debug.Assert(handler != null, "handler != null");
            Debug.Assert(terminationCondition != null, "terminationCondition != null");

            ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Gray);
            handler.DiscoverVertex(graph, u);

            var stack = new Stack<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>();

            TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
            stack.Push(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(u, false, default, outEdges));

            while (stack.Count > 0)
            {
                DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame = stack.Pop();

                throw new NotImplementedException();
            }
        }
    }
}
