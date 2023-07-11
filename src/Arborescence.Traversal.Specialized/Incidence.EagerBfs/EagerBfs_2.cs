namespace Arborescence.Traversal.Specialized.Incidence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Traversal.Incidence;

    public static class EagerBfs<TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where THandler : IBfsHandler<int, TEdge, TGraph> =>
            TraverseChecked(graph, source, vertexCount, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
            where THandler : IBfsHandler<int, TEdge, TGraph> =>
            TraverseChecked(graph, sources, vertexCount, handler, cancellationToken);

        internal static void TraverseChecked<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where THandler : IBfsHandler<int, TEdge, TGraph>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (handler is null)
                ArgumentNullExceptionHelpers.Throw(nameof(handler));

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                var colorByVertex = new Int32ColorDictionary(arrayFromPool);
                EagerBfs<int, TEdge, TEdgeEnumerator>.TraverseUnchecked(
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
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
            where THandler : IBfsHandler<int, TEdge, TGraph>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (handler is null)
                ArgumentNullExceptionHelpers.Throw(nameof(handler));

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                var colorByVertex = new Int32ColorDictionary(arrayFromPool);
                EagerBfs<int, TEdge, TEdgeEnumerator>.TraverseUnchecked(
                    graph, sources, colorByVertex, handler, cancellationToken);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
