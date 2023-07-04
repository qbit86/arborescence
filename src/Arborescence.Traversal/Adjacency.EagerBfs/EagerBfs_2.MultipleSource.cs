namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Threading;

    public static partial class EagerBfs<TVertex, TVertexEnumerator>
    {
        private static void TraverseUnchecked<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            var queue = new ValueQueue<TVertex>();
            try
            {
                using IEnumerator<TVertex> sourceEnumerator = sources.GetEnumerator();
                while (sourceEnumerator.MoveNext())
                {
                    TVertex source = sourceEnumerator.Current;
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
    }
}
