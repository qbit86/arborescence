namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static partial class EagerBfs<TVertex, TVertexEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, TVertex source, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            TraverseChecked(graph, source, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            TraverseChecked(graph, source, comparer, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            TraverseChecked(graph, source, colorByVertex, handler, cancellationToken);

        internal static void TraverseChecked<TGraph, THandler>(
            TGraph graph, TVertex source, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (source is null)
                ThrowHelper.ThrowArgumentNullException(nameof(source));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            Dictionary<TVertex, Color> colorByVertex = new();
            TraverseUnchecked(graph, source, colorByVertex, handler, cancellationToken);
        }

        internal static void TraverseChecked<TGraph, THandler>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (source is null)
                ThrowHelper.ThrowArgumentNullException(nameof(source));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            Dictionary<TVertex, Color> colorByVertex = new(comparer);
            TraverseUnchecked(graph, source, colorByVertex, handler, cancellationToken);
        }

        internal static void TraverseChecked<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (source is null)
                ThrowHelper.ThrowArgumentNullException(nameof(source));

            if (colorByVertex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(colorByVertex));

            if (handler is null)
                ThrowHelper.ThrowArgumentNullException(nameof(handler));

            TraverseUnchecked(graph, source, colorByVertex, handler, cancellationToken);
        }

        private static void TraverseUnchecked<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
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
    }
}
