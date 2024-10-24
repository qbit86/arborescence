namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
#if DEBUG
    using System.Diagnostics;
#endif

    public static partial class EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateVerticesChecked(graph, source, frontier);

        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateVerticesChecked(graph, source, frontier, comparer);

        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked(graph, source, frontier, exploredSet);

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            HashSet<TVertex> exploredSet = new();
            return EnumerateVerticesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateVerticesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            if (exploredSet is null)
                ArgumentNullExceptionHelpers.Throw(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            yield return source;
            frontier.AddOrThrow(source);

            while (frontier.TryTake(out var current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                var outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        if (!graph.TryGetHead(outEdges.Current, out var neighbor))
                            continue;
                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        frontier.AddOrThrow(neighbor);
                    }
                }
                finally
                {
                    outEdges.Dispose();
                }
            }
        }
    }
}
