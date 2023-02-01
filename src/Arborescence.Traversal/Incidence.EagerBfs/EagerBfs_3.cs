namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

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
            TGraph graph, ref ValueQueue<TVertex> queue, TColorMap colorByVertex, THandler handler)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            while (queue.TryTake(out TVertex current))
            {
#if DEBUG
                Debug.Assert(GetColorOrDefault(colorByVertex, current) != default);
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
                        Color neighborColor = GetColorOrDefault(colorByVertex, neighbor);
                        switch (neighborColor)
                        {
                            case Color.None or Color.White:
                                handler.OnTreeEdge(graph, edge);
                                colorByVertex[neighbor] = Color.Gray;
                                handler.OnDiscoverVertex(graph, neighbor);
                                queue.Add(neighbor);
                                break;
                            case Color.Gray:
                                handler.OnNonTreeGrayHeadEdge(graph, edge);
                                break;
                            case Color.Black:
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
