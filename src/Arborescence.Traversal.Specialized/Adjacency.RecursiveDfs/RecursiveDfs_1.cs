namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Traversal.Adjacency;

    public static class RecursiveDfs<TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<int>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<int, TVertexEnumerator>
            where THandler : IDfsHandler<int, Endpoints<int>, TGraph> =>
            TraverseChecked(graph, source, vertexCount, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<int, TVertexEnumerator>
            where TSourceCollection : IEnumerable<int>
            where THandler : IDfsHandler<int, Endpoints<int>, TGraph> =>
            TraverseChecked(graph, sources, vertexCount, handler, cancellationToken);

        internal static void TraverseChecked<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<int, TVertexEnumerator>
            where THandler : IDfsHandler<int, Endpoints<int>, TGraph>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            if (handler is null)
                ArgumentNullExceptionHelpers.Throw(nameof(handler));

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                var colorByVertex = new Int32ColorDictionary(arrayFromPool);
                RecursiveDfs<int, TVertexEnumerator>.TraverseUnchecked(
                    graph, source, colorByVertex, handler, cancellationToken);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static void TraverseChecked<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, int vertexCount, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<int, TVertexEnumerator>
            where TSourceCollection : IEnumerable<int>
            where THandler : IDfsHandler<int, Endpoints<int>, TGraph>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            if (handler is null)
                ArgumentNullExceptionHelpers.Throw(nameof(handler));

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                var colorByVertex = new Int32ColorDictionary(arrayFromPool);
                RecursiveDfs<int, TVertexEnumerator>.TraverseUnchecked(
                    graph, sources, colorByVertex, handler, cancellationToken);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
