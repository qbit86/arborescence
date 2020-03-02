namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Internal;

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

            List<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>> stack =
                ListCache<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Acquire();

            TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
            stack.Add(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(u, false, default, outEdges));

            while (stack.Count > 0)
            {
                DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
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
                    Color color = GetColorOrDefault(colorMap, v);
                    if (color == Color.None || color == Color.White)
                    {
                        handler.TreeEdge(graph, e);
                        stack.Add(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(u, true, e, edges));
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

            ListCache<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>.Release(stack);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
}
