namespace Arborescence.Traversal
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public readonly partial struct EagerBfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        private static void TraverseCore<TColorMap, THandler>(
            TGraph graph, Internal.Queue<TVertex> queue, TColorMap colorMap, THandler handler)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(graph != null, "graph != null");
            Debug.Assert(handler != null, "handler != null");

            try
            {
                while (queue.TryTake(out TVertex u))
                {
#if DEBUG
                    Debug.Assert(GetColorOrDefault(colorMap, u) != default);
#endif
                    handler.OnExamineVertex(graph, u);
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out TVertex v))
                            continue;

                        handler.OnExamineEdge(graph, e);
                        Color vColor = GetColorOrDefault(colorMap, v);
                        switch (vColor)
                        {
                            case Color.None:
                            case Color.White:
                                handler.OnTreeEdge(graph, e);
                                colorMap[v] = Color.Gray;
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

                    colorMap[u] = Color.Black;
                    handler.OnFinishVertex(graph, u);
                }
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorMap, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorMap.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
