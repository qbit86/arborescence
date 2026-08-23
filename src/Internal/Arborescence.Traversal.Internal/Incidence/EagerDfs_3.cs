namespace Arborescence.Traversal.Internal.Incidence
{
    using System.Collections.Generic;
    using System.Threading;

    internal static class EagerDfs<TVertex, TEdge, TEdgeEnumerator>
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
            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorByVertex, handler, cancellationToken);
        }

        internal static void TraverseUnchecked<TGraph, TVertexCollection, TColorMap, THandler>(
            TGraph graph, TVertexCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TVertexCollection : IEnumerable<TVertex>
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
                TraverseCore(graph, source, colorByVertex, handler, cancellationToken);
            }
        }

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
                var outEdges = graph.EnumerateOutEdges(vertex);
                stack.Add(new(vertex, outEdges));

                while (stack.TryTake(out var stackFrame))
                {
                    vertex = stackFrame.Vertex;
                    if (stackFrame.TryGetEdge(out var inEdge))
                        handler.OnFinishEdge(graph, inEdge);

                    var edges = stackFrame.EdgeEnumerator;
                    while (true)
                    {
                        if (!edges.MoveNext())
                        {
                            edges.Dispose();
                            break;
                        }

                        var edge = edges.Current;
                        if (!graph.TryGetHead(edge, out var neighbor))
                            continue;

                        handler.OnExamineEdge(graph, edge);
                        var color = colorByVertex.GetValueOrDefault(neighbor, Color.None);
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
                while (stack.TryTake(out var stackFrame))
                    stackFrame.EdgeEnumerator.Dispose();
                stack.Dispose();
            }
        }

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
