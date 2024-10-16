namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableDfs<TVertex, TNeighborEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
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
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex> =>
            EnumerateVerticesChecked(graph, sources);

        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
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
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex> =>
            EnumerateVerticesChecked(graph, sources, comparer);

        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
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
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked(graph, sources, exploredSet);

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            HashSet<TVertex> exploredSet = new();
            return EnumerateVerticesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateVerticesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesChecked<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (exploredSet is null)
                ArgumentNullExceptionHelpers.Throw(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            var sourceEnumerator = sources.GetEnumerator();
            var stack = new ValueStack<TNeighborEnumerator>();
            try
            {
                while (sourceEnumerator.MoveNext())
                {
                    var source = sourceEnumerator.Current;
                    if (!exploredSet.Add(source))
                        continue;
                    yield return source;
                    stack.Add(graph.EnumerateOutNeighbors(source));

                    while (stack.TryTake(out var neighborEnumerator))
                    {
                        if (!neighborEnumerator.MoveNext())
                        {
                            neighborEnumerator.Dispose();
                            continue;
                        }

                        var neighbor = neighborEnumerator.Current;
                        stack.Add(neighborEnumerator);
                        if (!exploredSet.Add(neighbor))
                            continue;
                        yield return neighbor;
                        stack.Add(graph.EnumerateOutNeighbors(neighbor));
                    }
                }
            }
            finally
            {
                while (stack.TryTake(out var stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
                sourceEnumerator.Dispose();
            }
        }
    }
}
