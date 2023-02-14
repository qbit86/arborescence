namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableDfs<TVertex, TNeighborEnumerator>
    {
        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph>(
            TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator> =>
            EnumerateEdgesChecked(graph, source);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator> =>
            EnumerateEdgesChecked(graph, source, comparer);

        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerable collection of the endpoints of a search tree edges.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<TVertex>> EnumerateEdges<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex> =>
            EnumerateEdgesChecked(graph, source, exploredSet);

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<TGraph>(
            TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            HashSet<TVertex> exploredSet = new();
            return EnumerateEdgesIterator(graph, source, exploredSet);
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateEdgesIterator(graph, source, exploredSet);
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesChecked<TGraph, TExploredSet>(
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

        private static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (!exploredSet.Add(source))
                yield break;
            var stack = new ValueStack<StackFrame>();
            try
            {
                stack.Add(new(source, graph.EnumerateOutNeighbors(source)));

                while (stack.TryTake(out StackFrame stackFrame))
                {
                    (TVertex current, TNeighborEnumerator neighborEnumerator) = stackFrame;
                    if (!neighborEnumerator.MoveNext())
                    {
                        neighborEnumerator.Dispose();
                        continue;
                    }

                    TVertex neighbor = neighborEnumerator.Current;
                    stack.Add(stackFrame with { NeighborEnumerator = neighborEnumerator });
                    if (exploredSet.Contains(neighbor))
                        continue;

                    yield return new(current, neighbor);
                    exploredSet.Add(neighbor);
                    stack.Add(new(neighbor, graph.EnumerateOutNeighbors(neighbor)));
                }
            }
            finally
            {
                while (stack.TryTake(out StackFrame stackFrame))
                    stackFrame.Dispose();
                stack.Dispose();
            }
        }
    }
}
