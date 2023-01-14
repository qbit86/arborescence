namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TVertexEnumerator, TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked<TVertex, TVertexEnumerator, TGraph, TSourceEnumerator, TExploredSet>(
                graph, sources, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<
            TVertex, TVertexEnumerator, TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            Traversal.Queue<TVertex> frontier = new();
            return EnumerableGenericSearch.EnumerateVerticesIterator<
                TVertex, TVertexEnumerator, TGraph, TSourceEnumerator, Traversal.Queue<TVertex>, TExploredSet>(
                graph, sources, frontier, exploredSet);
        }
    }
}
