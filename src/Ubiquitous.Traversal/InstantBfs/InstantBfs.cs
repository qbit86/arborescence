namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Collections;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct InstantBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphPolicy, TColorMapPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IHeadPolicy<TGraph, TVertex, TEdge>
        where TColorMapPolicy : IMapPolicy<TColorMap, TVertex, Color>
    {
        private TGraphPolicy GraphPolicy { get; }
        private TColorMapPolicy ColorMapPolicy { get; }

        public InstantBfs(TGraphPolicy graphPolicy, TColorMapPolicy colorMapPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (colorMapPolicy == null)
                throw new ArgumentNullException(nameof(colorMapPolicy));

            GraphPolicy = graphPolicy;
            ColorMapPolicy = colorMapPolicy;
        }

        private void TraverseCore<TContainer, THandler>(
            TGraph graph, TContainer queue, TColorMap colorMap, THandler handler)
            where TContainer : IContainer<TVertex>, IDisposable
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(queue != null, "queue != null");
            Debug.Assert(handler != null, "handler != null");

            using (queue)
            {
                while (queue.TryTake(out TVertex u))
                {
                    handler.OnExamineVertex(graph, u);
                    TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                            continue;

                        handler.OnExamineEdge(graph, e);
                        Color vColor = GetColorOrDefault(colorMap, v);
                        switch (vColor)
                        {
                            case Color.None:
                            case Color.White:
                                handler.OnTreeEdge(graph, e);
                                ColorMapPolicy.AddOrUpdate(colorMap, v, Color.Gray);
                                handler.OnDiscoverVertex(graph, v);
                                queue.Add(v);
                                break;
                            case Color.Gray:
                                handler.OnNonTreeGrayHeadEdge(graph, e);
                                break;
                            case Color.Black:
                                handler.OnNonTreeBlackHeadEdge(graph, e);
                                break;
                        }
                    }

                    ColorMapPolicy.AddOrUpdate(colorMap, u, Color.Black);
                    handler.OnFinishVertex(graph, u);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Color GetColorOrDefault(TColorMap colorMap, TVertex vertex) =>
            ColorMapPolicy.TryGetValue(colorMap, vertex, out Color result) ? result : Color.None;
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
