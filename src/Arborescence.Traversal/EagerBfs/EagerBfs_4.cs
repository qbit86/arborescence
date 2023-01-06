namespace Arborescence.Traversal
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the BFS algorithm — breadth-first traversal of the graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public readonly partial struct EagerBfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TVertex : notnull
    {
        private static void TraverseCore<TColorMap, THandler>(
            TGraph graph, Internal.Queue<TVertex> queue, TColorMap colorByVertex, THandler handler)
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TGraph, TVertex, TEdge>
        {
            try
            {
                while (queue.TryTake(out TVertex u))
                {
#if DEBUG
                    Debug.Assert(GetColorOrDefault(colorByVertex, u) != default);
#endif
                    handler.OnExamineVertex(graph, u);
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    try
                    {
                        while (outEdges.MoveNext())
                        {
                            TEdge e = outEdges.Current;
                            if (!graph.TryGetHead(e, out TVertex? v))
                                continue;

                            handler.OnExamineEdge(graph, e);
                            Color vColor = GetColorOrDefault(colorByVertex, v);
                            switch (vColor)
                            {
                                case Color.None:
                                case Color.White:
                                    handler.OnTreeEdge(graph, e);
                                    colorByVertex[v] = Color.Gray;
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
                    }
                    finally
                    {
                        outEdges.Dispose();
                    }

                    colorByVertex[u] = Color.Black;
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
        private static Color GetColorOrDefault<TColorMap>(TColorMap colorByVertex, TVertex vertex)
            where TColorMap : IDictionary<TVertex, Color> =>
            colorByVertex.TryGetValue(vertex, out Color result) ? result : Color.None;
    }
}
