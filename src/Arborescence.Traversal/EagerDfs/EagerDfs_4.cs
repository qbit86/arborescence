namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a non-recursive fashion.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public readonly partial struct EagerDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TVertex : notnull
    {
        private static readonly Func<TGraph, TVertex, bool> s_false = (_, _) => false;

        private static void TraverseCore<TColorMap, THandler>(TGraph graph, TVertex u, TColorMap colorByVertex,
            THandler handler, Func<TGraph, TVertex, bool> terminationCondition)
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

            var stack = new Internal.Stack<StackFrame<TVertex, TEdge, TEdgeEnumerator>>();
            try
            {
                TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                stack.Add(new(u, outEdges));

                while (stack.TryTake(out StackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame))
                {
                    u = stackFrame.Vertex;
                    if (stackFrame.TryGetEdge(out TEdge inEdge))
                        handler.OnFinishEdge(graph, inEdge);

                    TEdgeEnumerator edges = stackFrame.EdgeEnumerator;
                    while (true)
                    {
                        if (!edges.MoveNext())
                        {
                            outEdges.Dispose();
                            break;
                        }

                        TEdge e = edges.Current;
                        if (!graph.TryGetHead(e, out TVertex? v))
                            continue;

                        handler.OnExamineEdge(graph, e);
                        Color color = GetColorOrDefault(colorByVertex, v);
                        if (color == Color.None || color == Color.White)
                        {
                            handler.OnTreeEdge(graph, e);
                            stack.Add(new(u, e, edges));
                            u = v;
                            colorByVertex[u] = Color.Gray;
                            handler.OnDiscoverVertex(graph, u);

                            edges = graph.EnumerateOutEdges(u);
                            if (terminationCondition(graph, u))
                            {
                                colorByVertex[u] = Color.Black;
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

                    colorByVertex[u] = Color.Black;
                    handler.OnFinishVertex(graph, u);
                }
            }
            catch
            {
                while (stack.TryTake(out StackFrame<TVertex, TEdge, TEdgeEnumerator> stackFrame))
                    stackFrame.EdgeEnumerator.Dispose();
                throw;
            }
            finally
            {
                stack.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
