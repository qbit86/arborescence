namespace Arborescence.Traversal.Internal.Adjacency
{
    using System.Collections.Generic;
    using System.Threading;
#if DEBUG
    using System.Diagnostics;
#endif

    internal static class EagerBfs<TVertex, TNeighborEnumerator>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        internal static void TraverseUnchecked<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            colorByVertex[source] = Color.Gray;
            handler.OnDiscoverVertex(graph, source);
            if (cancellationToken.IsCancellationRequested)
            {
                colorByVertex[source] = Color.Black;
                handler.OnFinishVertex(graph, source);
                return;
            }

            var queue = new ValueQueue<TVertex>();
            try
            {
                queue.Add(source);
                Traverse(graph, ref queue, colorByVertex, handler, cancellationToken);
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }

        internal static void TraverseUnchecked<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            var queue = new ValueQueue<TVertex>();
            try
            {
                using var sourceEnumerator = sources.GetEnumerator();
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    colorByVertex[source] = Color.Gray;
                    handler.OnDiscoverVertex(graph, source);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        colorByVertex[source] = Color.Black;
                        handler.OnFinishVertex(graph, source);
                        return;
                    }

                    queue.Add(source);
                }

                Traverse(graph, ref queue, colorByVertex, handler, cancellationToken);
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }

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
