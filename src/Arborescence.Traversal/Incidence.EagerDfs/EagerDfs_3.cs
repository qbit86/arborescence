namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a non-recursive manner.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static partial class EagerDfs<TVertex, TEdge, TEdgeEnumerator>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        private static void TraverseCore<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex vertex, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TVertex, TEdge, TGraph>
        {
            colorByVertex[vertex] = Color.Gray;
            handler.OnDiscoverVertex(graph, vertex);
            if (cancellationToken.IsCancellationRequested)
            {
                colorByVertex[vertex] = Color.Black;
                handler.OnFinishVertex(graph, vertex);
                return;
            }

            var stack = new ValueStack<StackFrame>();
            try
            {
                TEdgeEnumerator outEdges = graph.EnumerateOutEdges(vertex);
                stack.Add(new(vertex, outEdges));

                while (stack.TryTake(out StackFrame stackFrame))
                {
                    vertex = stackFrame.Vertex;
                    if (stackFrame.TryGetEdge(out TEdge inEdge))
                        handler.OnFinishEdge(graph, inEdge);

                    TEdgeEnumerator edges = stackFrame.EdgeEnumerator;
                    while (true)
                    {
                        if (!edges.MoveNext())
                        {
                            edges.Dispose();
                            break;
                        }

                        TEdge edge = edges.Current;
                        if (!graph.TryGetHead(edge, out TVertex? neighbor))
                            continue;

                        handler.OnExamineEdge(graph, edge);
                        Color color = GetColorOrDefault(colorByVertex, neighbor);
                        if (color is Color.None or Color.White)
                        {
                            handler.OnTreeEdge(graph, edge);
                            stack.Add(new(vertex, edge, edges));
                            vertex = neighbor;
                            colorByVertex[vertex] = Color.Gray;
                            handler.OnDiscoverVertex(graph, vertex);
                            edges = graph.EnumerateOutEdges(vertex);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                colorByVertex[vertex] = Color.Black;
                                handler.OnFinishVertex(graph, vertex);
                                return;
                            }
                        }
                        else
                        {
                            if (color is Color.Gray)
                                handler.OnBackEdge(graph, edge);
                            else
                                handler.OnForwardOrCrossEdge(graph, edge);
                            handler.OnFinishEdge(graph, edge);
                        }
                    }

                    colorByVertex[vertex] = Color.Black;
                    handler.OnFinishVertex(graph, vertex);
                }
            }
            finally
            {
                while (stack.TryTake(out StackFrame stackFrame))
                    stackFrame.EdgeEnumerator.Dispose();
                stack.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;

        private readonly struct StackFrame
        {
            private readonly TEdge _edge;
            private readonly bool _hasEdge;

            internal StackFrame(TVertex vertex, TEdgeEnumerator edgeEnumerator)
            {
                _hasEdge = false;
                _edge = default!;
                Vertex = vertex;
                EdgeEnumerator = edgeEnumerator;
            }

            internal StackFrame(TVertex vertex, TEdge edge, TEdgeEnumerator edgeEnumerator)
            {
                _hasEdge = true;
                _edge = edge;
                Vertex = vertex;
                EdgeEnumerator = edgeEnumerator;
            }

            internal TVertex Vertex { get; }
            internal TEdgeEnumerator EdgeEnumerator { get; }

            internal bool TryGetEdge(out TEdge edge)
            {
                edge = _edge;
                return _hasEdge;
            }
        }
    }
}
