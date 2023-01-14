#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET47_OR_GREATER
namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs<TVertex, TNeighborEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerateEdgesChecked(graph, sources);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerateEdgesChecked(graph, sources, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateEdgesChecked(graph, sources, exploredSet);

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            HashSet<TVertex> exploredSet = new();
            return EnumerateEdgesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            HashSet<TVertex> exploredSet = new(comparer);
            return EnumerateEdgesIterator(graph, sources, exploredSet);
        }

        internal static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesChecked<
            TGraph, TSourceEnumerator, TExploredSet>(
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

            return EnumerateEdgesIterator(graph, sources, exploredSet);
        }

        private static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdgesIterator<
            TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            IEnumerator<(TVertex Tail, TVertex Head)> enumerator = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateEdgesIterator(graph, sources, frontier, exploredSet);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}
#endif
