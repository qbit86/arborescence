namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Traversal.Adjacency;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a non-recursive manner.
    /// </summary>
    /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
    public static class EagerDfs<TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<int>
    {
        /// <summary>
        /// Traverses the graph in a depth-first order starting from the single source
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="cancellationToken">Optional token used to stop the traversal.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="source"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<int, TVertexEnumerator>
            where THandler : IDfsHandler<int, Endpoints<int>, TGraph> =>
            TraverseChecked(graph, source, vertexCount, handler, cancellationToken);

        /// <summary>
        /// Traverses the graph in a depth-first order starting from the multiple sources
        /// until the search tree is built or until the search is canceled.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <param name="handler">
        /// The <see cref="IDfsHandler{TGraph,TVertex,TEdge}"/> implementation to use
        /// for the actions taken during the graph traversal.
        /// </param>
        /// <param name="cancellationToken">Optional token used to stop the traversal.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="THandler">The type of the events handler.</typeparam>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
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
#if DEBUG && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
            Array.Fill(arrayFromPool, (byte)Color.White, vertexCount, arrayFromPool.Length - vertexCount);
#endif
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                var colorByVertex = new Int32ColorDictionary(arrayFromPool);
                EagerDfs<int, TVertexEnumerator>.TraverseUnchecked(
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
#if DEBUG && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
            Array.Fill(arrayFromPool, (byte)Color.White, vertexCount, arrayFromPool.Length - vertexCount);
#endif
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                var colorByVertex = new Int32ColorDictionary(arrayFromPool);
                EagerDfs<int, TVertexEnumerator>.TraverseUnchecked(
                    graph, sources, colorByVertex, handler, cancellationToken);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
