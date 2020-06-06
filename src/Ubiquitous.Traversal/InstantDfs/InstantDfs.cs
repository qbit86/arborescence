namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Collections;

    public static class InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>
            Create<TGraphPolicy, TColorMapPolicy>(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
            where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetHeadPolicy<TGraph, TVertex, TEdge>
            where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
        {
            return new InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap, TGraphPolicy, TColorMapPolicy>(
                graphPolicy, colorMapPolicy);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct InstantDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetHeadPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }

        public InstantDfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        private void TraverseCore<THandler>(TGraph graph, TVertex u, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(handler != null, "handler != null");
            Debug.Assert(terminationCondition != null, "terminationCondition != null");

            ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Gray);
            handler.OnDiscoverVertex(graph, u);

            if (terminationCondition(graph, u))
            {
                ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                handler.OnFinishVertex(graph, u);
                return;
            }

            var stack = new ValueStack<DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>>();

            TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
            stack.Add(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(u, false, default, outEdges));

            while (stack.TryTake(out DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame))
            {
                u = stackFrame.Vertex;
                if (stackFrame.HasEdge)
                    handler.OnFinishEdge(graph, stackFrame.Edge);

                TEdgeEnumerator edges = stackFrame.EdgeEnumerator;
                while (edges.MoveNext())
                {
                    TEdge e = edges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    handler.OnExamineEdge(graph, e);
                    Color color = GetColorOrDefault(colorMap, v);
                    if (color == Color.None || color == Color.White)
                    {
                        handler.OnTreeEdge(graph, e);
                        stack.Add(new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(u, true, e, edges));
                        u = v;
                        ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Gray);
                        handler.OnDiscoverVertex(graph, u);

                        edges = GraphPolicy.EnumerateOutEdges(graph, u);
                        if (terminationCondition(graph, u))
                        {
                            ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                            handler.OnFinishVertex(graph, u);
                            return;
                        }
                    }
                    else
                    {
                        if (color == Color.Gray)
                            handler.OnBackEdge(graph, e);
                        else
                            handler.OnForwardOrCrossEdge(graph, e);
                        handler.OnFinishEdge(graph, e);
                    }
                }

                ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                handler.OnFinishVertex(graph, u);
            }

            stack.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
