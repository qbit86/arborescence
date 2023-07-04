namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static partial class EagerBfs<TVertex, TVertexEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            TraverseChecked(graph, sources, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, IEqualityComparer<TVertex> comparer, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            TraverseChecked(graph, sources, comparer, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            TraverseChecked(graph, sources, colorByVertex, handler, cancellationToken);

        internal static void TraverseChecked<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            Dictionary<TVertex, Color> colorByVertex = new();
            TraverseUnchecked(graph, sources, colorByVertex, handler, cancellationToken);
        }

        internal static void TraverseChecked<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, IEqualityComparer<TVertex> comparer, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            Dictionary<TVertex, Color> colorByVertex = new(comparer);
            TraverseUnchecked(graph, sources, colorByVertex, handler, cancellationToken);
        }

        internal static void TraverseChecked<TGraph, TSourceCollection, TColorMap, THandler>(
            TGraph graph, TSourceCollection sources, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (colorByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(colorByVertex));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            TraverseUnchecked(graph, sources, colorByVertex, handler, cancellationToken);
        }

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
