namespace Arborescence.Traversal.Adjacency
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
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static partial class EagerBfs<TVertex, TNeighborEnumerator>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        private static void Traverse<TGraph, TColorMap, THandler>(
            TGraph graph, ref ValueQueue<TVertex> queue, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            while (queue.TryTake(out var current))
            {
#if DEBUG
                Debug.Assert(colorByVertex.GetValueOrDefault(current, Color.None) != default);
#endif
                handler.OnExamineVertex(graph, current);
                var outNeighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (outNeighbors.MoveNext())
                    {
                        var neighbor = outNeighbors.Current!;
                        var edge = Endpoints.Create(current, neighbor);
                        handler.OnExamineEdge(graph, edge);
                        var neighborColor = colorByVertex.GetValueOrDefault(neighbor, Color.None);
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
                    outNeighbors.Dispose();
                }

                colorByVertex[current] = Color.Black;
                handler.OnFinishVertex(graph, current);
            }
        }
    }
}
