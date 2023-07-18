namespace Arborescence.Traversal.Specialized.Incidence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph — implemented as iterator.
    /// </summary>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static class EnumerableDfs<TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator> =>
            EnumerateVerticesChecked(graph, source, vertexCount);

        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int> =>
            EnumerateVerticesChecked(graph, sources, vertexCount);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator> =>
            EnumerateEdgesChecked(graph, source, vertexCount);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int> =>
            EnumerateEdgesChecked(graph, sources, vertexCount);

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableDfs<int, TEdge, TEdgeEnumerator>
                    .EnumerateVerticesIterator(graph, source, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

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
                return Arborescence.Traversal.Incidence.EnumerableDfs<int, TEdge, TEdgeEnumerator>
                    .EnumerateVerticesIterator(graph, sources, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableDfs<int, TEdge, TEdgeEnumerator>
                    .EnumerateEdgesIterator(graph, source, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableDfs<int, TEdge, TEdgeEnumerator>
                    .EnumerateEdgesIterator(graph, sources, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
