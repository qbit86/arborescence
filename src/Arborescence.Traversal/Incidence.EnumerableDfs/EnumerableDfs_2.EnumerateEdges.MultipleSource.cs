namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableDfs<TVertex, TEdge>
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
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerableDfs<TVertex, TEdge, IEnumerator<TEdge>>.EnumerateEdgesChecked(graph, sources);

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
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerableDfs<TVertex, TEdge, IEnumerator<TEdge>>.EnumerateEdgesChecked(graph, sources, comparer);

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
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, IEnumerator<TEdge>>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableDfs<TVertex, TEdge, IEnumerator<TEdge>>.EnumerateEdgesChecked(graph, sources, exploredSet);
    }
}
