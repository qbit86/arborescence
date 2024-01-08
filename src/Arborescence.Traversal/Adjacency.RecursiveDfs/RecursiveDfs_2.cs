namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a recursive manner using the program stack.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static partial class RecursiveDfs<TVertex, TNeighborEnumerator>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        private static void Visit<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex vertex, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            colorByVertex[vertex] = Color.Gray;
            handler.OnDiscoverVertex(graph, vertex);

            if (cancellationToken.IsCancellationRequested)
            {
                colorByVertex[vertex] = Color.Black;
                handler.OnFinishVertex(graph, vertex);
                return;
            }

            TNeighborEnumerator outNeighbors = graph.EnumerateOutNeighbors(vertex);
            try
            {
                while (outNeighbors.MoveNext())
                {
                    TVertex neighbor = outNeighbors.Current!;
                    var edge = Endpoints.Create(vertex, neighbor);
                    handler.OnExamineEdge(graph, edge);
                    Color neighborColor = colorByVertex.GetValueOrDefault(neighbor, Color.None);
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
                outNeighbors.Dispose();
            }

            colorByVertex[vertex] = Color.Black;
            handler.OnFinishVertex(graph, vertex);
        }
    }
}
