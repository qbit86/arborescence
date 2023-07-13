namespace Arborescence.Traversal.Specialized.Incidence
{
    using System;
    using System.Buffers;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableGenericSearch<TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerateVerticesChecked(graph, source, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerateVerticesChecked(graph, sources, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerateEdgesChecked(graph, source, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerateEdgesChecked(graph, sources, frontier, vertexCount);

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
                    .EnumerateVerticesIterator(graph, source, frontier, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
                    .EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
                    .EnumerateEdgesIterator(graph, source, frontier, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<
            TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
                    .EnumerateEdgesIterator(graph, sources, frontier, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
