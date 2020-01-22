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
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, s_false);
        }

        public void Traverse<THandler>(TGraph graph, TVertex startVertex, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            handler.StartVertex(graph, startVertex);
            TraverseCore(graph, startVertex, colorMap, handler, terminationCondition ?? s_false);
        }

        private void TraverseCore<THandler>(TGraph graph, TVertex u, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(handler != null, "handler != null");
            Debug.Assert(terminationCondition != null, "terminationCondition != null");

            ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Gray);
            handler.DiscoverVertex(graph, u);

            if (terminationCondition(graph, u))
            {
                ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                handler.FinishVertex(graph, u);
                return;
            }

            var stack = new Stack<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>();

            TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
            stack.Push(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(u, false, default, outEdges));

            while (stack.Count > 0)
            {
                DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame = stack.Pop();
                u = stackFrame.Vertex;
                if (stackFrame.HasEdge)
                    handler.FinishEdge(graph, stackFrame.Edge);

                TEdgeEnumerator edges = stackFrame.EdgeEnumerator;
                while (edges.MoveNext())
                {
                    TEdge e = edges.Current;
                    if (!GraphPolicy.TryGetTarget(graph, e, out TVertex v))
                        continue;

                    handler.ExamineEdge(graph, e);
                    if (!ColorMapPolicy.TryGetValue(colorMap, v, out Color color))
                        color = Color.None;

                    if (color == Color.None || color == Color.White)
                    {
                        handler.TreeEdge(graph, e);
                        stack.Push(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(u, true, e, edges));
                        u = v;
                        ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Gray);
                        handler.DiscoverVertex(graph, u);

                        edges = GraphPolicy.EnumerateOutEdges(graph, u);
                        if (terminationCondition(graph, u))
                        {
                            ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                            handler.FinishVertex(graph, u);
                            return;
                        }
                    }
                    else
                    {
                        if (color == Color.Gray)
                            handler.BackEdge(graph, e);
                        else
                            handler.ForwardOrCrossEdge(graph, e);
                        handler.FinishEdge(graph, e);
                    }
                }

                ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                handler.FinishVertex(graph, u);
            }
        }
    }
}
