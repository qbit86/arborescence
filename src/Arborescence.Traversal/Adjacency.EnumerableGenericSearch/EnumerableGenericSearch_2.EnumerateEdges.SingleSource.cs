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
        /// Enumerates edges of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <returns>An enumerator to enumerate the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<Endpoints<TVertex>> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateEdgesChecked(graph, source, frontier);

        /// <summary>
        /// Enumerates edges of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <returns>An enumerator to enumerate the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<Endpoints<TVertex>> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerateEdgesChecked(graph, source, frontier, comparer);

        /// <summary>
        /// Enumerates edges of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the endpoints of a search tree edges.</returns>
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
        public static IEnumerator<Endpoints<TVertex>> EnumerateEdges<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateEdgesChecked(graph, source, frontier, exploredSet);

        internal static IEnumerator<Endpoints<TVertex>> EnumerateEdgesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            HashSet<TVertex> exploredSet = new();
            return EnumerateEdgesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<Endpoints<TVertex>> EnumerateEdgesChecked<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateEdgesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<Endpoints<TVertex>> EnumerateEdgesChecked<
            TGraph, TFrontier, TExploredSet>(TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateEdgesIterator(graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<Endpoints<TVertex>> EnumerateEdgesIterator<
            TGraph, TFrontier, TExploredSet>(TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            exploredSet.Add(source);
            frontier.AddOrThrow(source);

            while (frontier.TryTake(out TVertex? current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                TNeighborEnumerator neighbors = graph.EnumerateNeighbors(current);
                try
                {
                    while (neighbors.MoveNext())
                    {
                        TVertex neighbor = neighbors.Current;
                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return new(current, neighbor);
                        exploredSet.Add(neighbor);
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
