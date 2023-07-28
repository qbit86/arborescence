namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
#if DEBUG
    using System.Diagnostics;
#endif

    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateVerticesChecked(graph, sources, frontier);

        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateVerticesChecked(graph, sources, frontier, comparer);

        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TSourceCollection, TFrontier, TExploredSet>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked(graph, sources, frontier, exploredSet);

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            HashSet<TVertex> exploredSet = new();
            return EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<
            TGraph, TSourceCollection, TFrontier, TExploredSet>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (frontier is null)
                ArgumentNullExceptionHelpers.Throw(nameof(frontier));

            if (exploredSet is null)
                ArgumentNullExceptionHelpers.Throw(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<
            TGraph, TSourceCollection, TFrontier, TExploredSet>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            IEnumerator<TVertex> sourceEnumerator = sources.GetEnumerator();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    TVertex source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    yield return source;
                    frontier.AddOrThrow(source);
                }
            }
            finally
            {
                sourceEnumerator.Dispose();
            }

            while (frontier.TryTake(out TVertex? current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                TNeighborEnumerator neighbors = graph.EnumerateOutNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        TVertex neighbor = neighbors.Current;
                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        frontier.AddOrThrow(neighbor);
                    }
                }
                finally
                {
                    neighbors.Dispose();
                }
            }
        }
    }
}
