namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TExploredSet>(TGraph graph, TVertex source, TExploredSet exploredSet)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked<TVertex, TNeighborEnumerator, TGraph, TExploredSet>(
                graph, source, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<
            TVertex, TNeighborEnumerator, TGraph, TExploredSet>(TGraph graph, TVertex source, TExploredSet exploredSet)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            Traversal.Queue<TVertex> frontier = new();
            return EnumerableGenericSearch.EnumerateVerticesIterator<
                TVertex, TNeighborEnumerator, TGraph, Traversal.Queue<TVertex>, TExploredSet>(
                graph, source, frontier, exploredSet);
        }
    }
}
