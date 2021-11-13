namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a recursive fashion.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public readonly partial struct RecursiveDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TGraph : IIncidenceGraph<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TVertex : notnull
    {
        private static readonly Func<TGraph, TVertex, bool> s_false = (g, v) => false;

        private static void TraverseCore<TColorMap, THandler>(
            TGraph graph, TVertex u, TColorMap colorByVertex, THandler handler,
            Func<TGraph, TVertex, bool> terminationCondition)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            colorByVertex[u] = Color.Gray;
            handler.OnDiscoverVertex(graph, u);

            if (terminationCondition(graph, u))
            {
                colorByVertex[u] = Color.Black;
                handler.OnFinishVertex(graph, u);
                return;
            }

            TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
            try
            {
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!graph.TryGetHead(e, out TVertex v))
                        continue;

                    handler.OnExamineEdge(graph, e);
                    Color color = GetColorOrDefault(colorByVertex, v);
                    if (color == Color.None || color == Color.White)
                    {
                        handler.OnTreeEdge(graph, e);
                        TraverseCore(graph, v, colorByVertex, handler, terminationCondition);
                    }
                    else if (color == Color.Gray)
                        handler.OnBackEdge(graph, e);
                    else
                        handler.OnForwardOrCrossEdge(graph, e);

                    handler.OnFinishEdge(graph, e);
                }
            }
            finally
            {
                outEdges.Dispose();
            }

            colorByVertex[u] = Color.Black;
            handler.OnFinishVertex(graph, u);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
