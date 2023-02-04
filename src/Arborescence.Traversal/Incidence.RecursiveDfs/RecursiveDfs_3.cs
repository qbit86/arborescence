namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a recursive manner using the program stack.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static partial class RecursiveDfs<TVertex, TEdge, TEdgeEnumerator>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        private static void Visit<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex vertex, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            colorByVertex[vertex] = Color.Gray;
            handler.OnDiscoverVertex(graph, vertex);

            if (cancellationToken.IsCancellationRequested)
            {
                colorByVertex[vertex] = Color.Black;
                handler.OnFinishVertex(graph, vertex);
                return;
            }

            TEdgeEnumerator outEdges = graph.EnumerateOutEdges(vertex);
            try
            {
                while (outEdges.MoveNext())
                {
                    TEdge edge = outEdges.Current;
                    if (!graph.TryGetHead(edge, out TVertex? neighbor))
                        continue;

                    handler.OnExamineEdge(graph, edge);
                    Color neighborColor = GetColorOrDefault(colorByVertex, neighbor);
                    switch (neighborColor)
                    {
                        case Color.None or Color.White:
                            handler.OnTreeEdge(graph, edge);
                            Visit(graph, neighbor, colorByVertex, handler, cancellationToken);
                            break;
                        case Color.Gray:
                            handler.OnBackEdge(graph, edge);
                            break;
                        default:
                            handler.OnForwardOrCrossEdge(graph, edge);
                            break;
                    }

                    handler.OnFinishEdge(graph, edge);
                }
            }
            finally
            {
                outEdges.Dispose();
            }

            colorByVertex[vertex] = Color.Black;
            handler.OnFinishVertex(graph, vertex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
