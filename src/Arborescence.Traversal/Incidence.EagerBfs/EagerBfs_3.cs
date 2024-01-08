namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Threading;
#if DEBUG
    using System.Diagnostics;
#endif

    /// <summary>
    /// Represents the BFS algorithm — breadth-first traversal of the graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static partial class EagerBfs<TVertex, TEdge, TEdgeEnumerator>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        private static void Traverse<TGraph, TColorMap, THandler>(
            TGraph graph, ref ValueQueue<TVertex> queue, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, TEdge, TGraph>
        {
            while (queue.TryTake(out TVertex current))
            {
#if DEBUG
                Debug.Assert(colorByVertex.GetValueOrDefault(current, Color.None) != default);
#endif
                handler.OnExamineVertex(graph, current);
                TEdgeEnumerator outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        TEdge edge = outEdges.Current;
                        if (!graph.TryGetHead(edge, out TVertex? neighbor))
                            continue;

                        handler.OnExamineEdge(graph, edge);
                        Color neighborColor = colorByVertex.GetValueOrDefault(neighbor, Color.None);
                        switch (neighborColor)
                        {
                            case Color.None or Color.White:
                                handler.OnTreeEdge(graph, edge);
                                colorByVertex[neighbor] = Color.Gray;
                                handler.OnDiscoverVertex(graph, neighbor);
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    colorByVertex[current] = Color.Black;
                                    handler.OnFinishVertex(graph, current);
                                    return;
                                }

                                queue.Add(neighbor);
                                break;
                            case Color.Gray:
                                handler.OnNonTreeGrayHeadEdge(graph, edge);
                                break;
                            default:
                                handler.OnNonTreeBlackHeadEdge(graph, edge);
                                break;
                        }
                    }
                }
                finally
                {
                    outEdges.Dispose();
                }

                colorByVertex[current] = Color.Black;
                handler.OnFinishVertex(graph, current);
            }
        }
    }
}
