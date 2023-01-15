#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET47_OR_GREATER
namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs<TVertex, TNeighborEnumerator>
    {
        /// <summary>
        /// Enumerates edges of the graph in a breadth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <returns>An enumerator to enumerate the endpoints of a breadth-first search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph>(
            TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator> =>
            EnumerateEdgesChecked(graph, source);

        /// <summary>
        /// Enumerates edges of the graph in a breadth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <returns>An enumerator to enumerate the endpoints of a breadth-first search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator> =>
            EnumerateEdgesChecked(graph, source, comparer);

        /// <summary>
        /// Enumerates edges of the graph in a breadth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the endpoints of a breadth-first search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex> =>
            EnumerateEdgesChecked(graph, source, exploredSet);

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<TGraph>(
            TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            HashSet<TVertex> exploredSet = new();
            return EnumerateEdgesIterator(graph, source, exploredSet);
        }

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateEdgesIterator(graph, source, exploredSet);
        }

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateEdgesIterator(graph, source, exploredSet);
        }

        private static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            IEnumerator<(TVertex Tail, TVertex Head)> enumerator = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateEdgesIterator(graph, source, frontier, exploredSet);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}
#endif