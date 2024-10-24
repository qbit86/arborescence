namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a non-recursive manner.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static partial class EagerDfs<TVertex, TNeighborEnumerator>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        private static void TraverseCore<TGraph, TColorMap, THandler>(
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

            var stack = new ValueStack<StackFrame>();
            try
            {
                var outNeighbors = graph.EnumerateOutNeighbors(vertex);
                stack.Add(new(vertex, outNeighbors));

                while (stack.TryTake(out var stackFrame))
                {
                    vertex = stackFrame.Vertex;
                    if (stackFrame.TryGetNeighbor(out var inNeighbor))
                        handler.OnFinishEdge(graph, Endpoints.Create(inNeighbor, vertex));

                    var neighbors = stackFrame.NeighborEnumerator;
                    while (true)
                    {
                        if (!neighbors.MoveNext())
                        {
                            neighbors.Dispose();
                            break;
                        }

                        var neighbor = neighbors.Current!;
                        var edge = Endpoints.Create(vertex, neighbor);
                        handler.OnExamineEdge(graph, edge);
                        var color = colorByVertex.GetValueOrDefault(neighbor, Color.None);
                        if (color is Color.None or Color.White)
                        {
                            handler.OnTreeEdge(graph, edge);
                            stack.Add(new(vertex, neighbor, neighbors));
                            vertex = neighbor;
                            colorByVertex[vertex] = Color.Gray;
                            handler.OnDiscoverVertex(graph, vertex);
                            neighbors = graph.EnumerateOutNeighbors(vertex);
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
                    stackFrame.NeighborEnumerator.Dispose();
                stack.Dispose();
            }
        }

        private readonly struct StackFrame
        {
            private readonly TVertex _neighbor;
            private readonly bool _hasNeighbor;

            internal StackFrame(TVertex vertex, TNeighborEnumerator neighborEnumerator)
            {
                _hasNeighbor = false;
                _neighbor = default!;
                Vertex = vertex;
                NeighborEnumerator = neighborEnumerator;
            }

            internal StackFrame(TVertex vertex, TVertex neighbor, TNeighborEnumerator neighborEnumerator)
            {
                _hasNeighbor = true;
                _neighbor = neighbor;
                Vertex = vertex;
                NeighborEnumerator = neighborEnumerator;
            }

            internal TVertex Vertex { get; }
            internal TNeighborEnumerator NeighborEnumerator { get; }

            internal bool TryGetNeighbor(out TVertex neighbor)
            {
                neighbor = _neighbor;
                return _hasNeighbor;
            }
        }
    }
}
