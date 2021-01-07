namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public readonly partial struct RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        private static void TraverseCore<TColorMap, THandler>(
            TGraph graph, TVertex u, TColorMap colorMap, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            Debug.Assert(graph != null, "graph != null");
            Debug.Assert(handler != null, "handler != null");
            Debug.Assert(terminationCondition != null, "terminationCondition != null");

            colorMap[u] = Color.Gray;
            handler.OnDiscoverVertex(graph, u);

            if (terminationCondition(graph, u))
            {
                colorMap[u] = Color.Black;
                handler.OnFinishVertex(graph, u);
                return;
            }

            TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
            while (outEdges.MoveNext())
            {
                TEdge e = outEdges.Current;
                if (!graph.TryGetHead(e, out TVertex v))
                    continue;

                handler.OnExamineEdge(graph, e);
                Color color = GetColorOrDefault(colorMap, v);
                if (color == Color.None || color == Color.White)
                {
                    handler.OnTreeEdge(graph, e);
                    TraverseCore(graph, v, colorMap, handler, terminationCondition);
                }
                else if (color == Color.Gray)
                    handler.OnBackEdge(graph, e);
                else
                    handler.OnForwardOrCrossEdge(graph, e);

                handler.OnFinishEdge(graph, e);
            }

            colorMap[u] = Color.Black;
            handler.OnFinishVertex(graph, u);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorMap, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorMap.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
