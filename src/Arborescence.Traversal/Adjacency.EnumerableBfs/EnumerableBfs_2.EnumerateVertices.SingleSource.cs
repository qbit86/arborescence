namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableBfs<TVertex, TNeighborEnumerator>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph>(TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator> =>
            EnumerateVerticesChecked(graph, source);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator> =>
            EnumerateVerticesChecked(graph, source, comparer);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked(graph, source, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph>(TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            HashSet<TVertex> exploredSet = new();
            return EnumerateVerticesIterator(graph, source, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateVerticesIterator(graph, source, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, source, exploredSet);
        }

        private static IEnumerator<TVertex> EnumerateVerticesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            IEnumerator<TVertex> enumerator = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateVerticesIterator(graph, source, frontier, exploredSet);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}
