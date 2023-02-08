namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableDfs<TVertex>
    {
        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceCollection : IEnumerable<TVertex> =>
            EnumerableDfs<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(graph, sources);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceCollection : IEnumerable<TVertex> =>
            EnumerableDfs<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(graph, sources, comparer);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableDfs<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(graph, sources, exploredSet);
    }
}
