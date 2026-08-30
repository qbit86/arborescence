namespace Arborescence.Traversal.Internal.Incidence
{
    using System.Collections.Generic;
    using System.Threading;

    internal static class RecursiveDfs<TVertex, TEdge, TEdgeEnumerator>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        internal static void TraverseUnchecked<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TVertex, TEdge, TGraph>
        {
            var color = colorByVertex.GetValueOrDefault(source, Color.None);
            if (color is not (Color.None or Color.White))
                return;
            handler.OnStartVertex(graph, source);
            Visit(graph, source, colorByVertex, handler, cancellationToken);
        }

        internal static void TraverseUnchecked<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TVertex, TEdge, TGraph>
        {
            using var sourceEnumerator = sources.GetEnumerator();
            while (sourceEnumerator.MoveNext())
            {
                var source = sourceEnumerator.Current;
                var color = colorByVertex.GetValueOrDefault(source, Color.None);
                if (color is not (Color.None or Color.White))
                    continue;
                handler.OnStartVertex(graph, source);
                Visit(graph, source, colorByVertex, handler, cancellationToken);
            }
        }

        private static void Visit<TGraph, TColorMap, THandler>(
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

            var outEdges = graph.EnumerateOutEdges(vertex);
            try
            {
                while (outEdges.MoveNext())
                {
                    var edge = outEdges.Current;
                    if (!graph.TryGetHead(edge, out var neighbor))
                        continue;

                    handler.OnExamineEdge(graph, edge);
                    var neighborColor = colorByVertex.GetValueOrDefault(neighbor, Color.None);
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
    }
}
