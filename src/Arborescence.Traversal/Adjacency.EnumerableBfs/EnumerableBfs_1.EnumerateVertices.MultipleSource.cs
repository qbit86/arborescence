namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs<TVertex>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a breadth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceCollection : IEnumerable<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, sources);

        /// <summary>
        /// Enumerates vertices of the graph in a breadth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceCollection : IEnumerable<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, sources, comparer);

        /// <summary>
        /// Enumerates vertices of the graph in a breadth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TSourceCollection">The type of the source collection.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerable collection of the vertices of a search tree.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TVertex> EnumerateVertices<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, sources, exploredSet);
    }
}
