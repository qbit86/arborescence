namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableDfs<TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceEnumerator">The type of the source enumerator.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerateEdgesChecked(graph, sources);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceEnumerator">The type of the source enumerator.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerateEdgesChecked(graph, sources, comparer);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceEnumerator">The type of the source enumerator.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateEdgesChecked(graph, sources, exploredSet);

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            HashSet<TVertex> exploredSet = new();
            return EnumerateEdgesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateEdgesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<
            TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateEdgesIterator(graph, sources, exploredSet);
        }


        private static IEnumerable<TEdge> EnumerateEdgesIterator<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var stack = new ValueStack<TEdgeEnumerator>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    stack.Add(graph.EnumerateOutEdges(source));

                    while (stack.TryTake(out TEdgeEnumerator edgeEnumerator))
                    {
                        if (!edgeEnumerator.MoveNext())
                        {
                            edgeEnumerator.Dispose();
                            continue;
                        }

                        TEdge edge = edgeEnumerator.Current;
                        stack.Add(edgeEnumerator);

                        if (!graph.TryGetHead(edge, out TVertex? neighbor))
                            continue;

                        if (exploredSet.Contains(neighbor))
                            continue;

                        yield return edge;
                        exploredSet.Add(neighbor);
                        stack.Add(graph.EnumerateOutEdges(neighbor));
                    }
                }
            }
            finally
            {
                while (stack.TryTake(out TEdgeEnumerator stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
            }
        }
    }
}
