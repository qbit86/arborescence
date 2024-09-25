namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Traversal.Adjacency;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph — implemented as iterator.
    /// </summary>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static class EnumerableDfs<TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<int>
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
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator> =>
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
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
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
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph>(
            TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator> =>
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
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<int> =>
            EnumerateEdgesChecked(graph, sources, vertexCount);

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph>(
            TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            return EnumerateVerticesIterator();

            IEnumerable<int> EnumerateVerticesIterator()
            {
                byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
#if DEBUG && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
                Array.Fill(arrayFromPool, (byte)0xFF, vertexCount, arrayFromPool.Length - vertexCount);
#endif
                Array.Clear(arrayFromPool, 0, vertexCount);
                try
                {
                    Int32Set exploredSet = new(arrayFromPool);
                    var vertices = EnumerableDfs<int, TNeighborEnumerator>
                        .EnumerateVerticesIterator(graph, source, exploredSet);
                    foreach (int vertex in vertices)
                        yield return vertex;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(arrayFromPool);
                }
            }
        }

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            return EnumerateVerticesIterator();

            IEnumerable<int> EnumerateVerticesIterator()
            {
                byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
#if DEBUG && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
                Array.Fill(arrayFromPool, (byte)0xFF, vertexCount, arrayFromPool.Length - vertexCount);
#endif
                Array.Clear(arrayFromPool, 0, vertexCount);
                try
                {
                    Int32Set exploredSet = new(arrayFromPool);
                    var vertices = EnumerableDfs<int, TNeighborEnumerator>
                        .EnumerateVerticesIterator(graph, sources, exploredSet);
                    foreach (int vertex in vertices)
                        yield return vertex;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(arrayFromPool);
                }
            }
        }

        internal static IEnumerable<Endpoints<int>> EnumerateEdgesChecked<TGraph>(
            TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            return EnumerateEdgesIterator();

            IEnumerable<Endpoints<int>> EnumerateEdgesIterator()
            {
                byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
#if DEBUG && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
                Array.Fill(arrayFromPool, (byte)0xFF, vertexCount, arrayFromPool.Length - vertexCount);
#endif
                Array.Clear(arrayFromPool, 0, vertexCount);
                try
                {
                    Int32Set exploredSet = new(arrayFromPool);
                    var edges = EnumerableDfs<int, TNeighborEnumerator>
                        .EnumerateEdgesIterator(graph, source, exploredSet);
                    foreach (var edge in edges)
                        yield return edge;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(arrayFromPool);
                }
            }
        }

        internal static IEnumerable<Endpoints<int>> EnumerateEdgesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            return EnumerateEdgesIterator();

            IEnumerable<Endpoints<int>> EnumerateEdgesIterator()
            {
                byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
#if DEBUG && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
                Array.Fill(arrayFromPool, (byte)0xFF, vertexCount, arrayFromPool.Length - vertexCount);
#endif
                Array.Clear(arrayFromPool, 0, vertexCount);
                try
                {
                    Int32Set exploredSet = new(arrayFromPool);
                    var edges = EnumerableDfs<int, TNeighborEnumerator>
                        .EnumerateEdgesIterator(graph, sources, exploredSet);
                    foreach (var edge in edges)
                        yield return edge;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(arrayFromPool);
                }
            }
        }
    }
}
