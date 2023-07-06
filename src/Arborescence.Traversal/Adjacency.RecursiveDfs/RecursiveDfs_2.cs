namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a recursive manner using the program stack.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
    public static partial class RecursiveDfs<TVertex, TVertexEnumerator>
        where TVertex : notnull
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        private static void Visit<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex vertex, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
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

            TVertexEnumerator outNeighbors = graph.EnumerateOutNeighbors(vertex);
            try
            {
                while (outNeighbors.MoveNext())
                {
                    TVertex neighbor = outNeighbors.Current!;
                    var edge = Endpoints.Create(vertex, neighbor);
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
                outNeighbors.Dispose();
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
