namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs<TVertex, TNeighborEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a breadth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceEnumerator">The type of the source enumerator.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerateVerticesChecked(graph, sources);

        /// <summary>
        /// Enumerates vertices of the graph in a breadth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceEnumerator">The type of the source enumerator.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerateVerticesChecked(graph, sources, comparer);

        /// <summary>
        /// Enumerates vertices of the graph in a breadth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceEnumerator">The type of the source enumerator.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked(graph, sources, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            HashSet<TVertex> exploredSet = new();
            return EnumerateVerticesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateVerticesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, sources, exploredSet);
        }

        private static IEnumerator<TVertex> EnumerateVerticesIterator<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            IEnumerator<TVertex> enumerator = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}
