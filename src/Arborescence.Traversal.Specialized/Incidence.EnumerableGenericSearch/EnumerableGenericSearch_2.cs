namespace Arborescence.Traversal.Specialized.Incidence
{
    using System;
    using System.Buffers;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Traversal.Incidence;

    /// <summary>
    /// Represents the generic search algorithm — traversal of the graph
    /// where the order of exploring vertices is determined by the frontier implementation.
    /// </summary>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static class EnumerableGenericSearch<TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerateVerticesChecked(graph, source, frontier, vertexCount);

        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerateVerticesChecked(graph, sources, frontier, vertexCount);

        /// <summary>
        /// Enumerates edges of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerateEdgesChecked(graph, source, frontier, vertexCount);

        /// <summary>
        /// Enumerates edges of the graph in an order specified by the frontier starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
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
#if DEBUG && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
            Array.Fill(arrayFromPool, (byte)1, vertexCount, arrayFromPool.Length - vertexCount);
#endif
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
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
                return EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
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
                return EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
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
                return EnumerableGenericSearch<int, TEdge, TEdgeEnumerator>
                    .EnumerateEdgesIterator(graph, sources, frontier, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
