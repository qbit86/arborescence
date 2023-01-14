namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableGenericSearch
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TSourceEnumerator, TFrontier>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, TNeighborEnumerator>.EnumerateVerticesChecked(
                graph, sources, frontier);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TSourceEnumerator, TFrontier>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, TNeighborEnumerator>.EnumerateVerticesChecked(
                graph, sources, frontier, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableGenericSearch<TVertex, TNeighborEnumerator>.EnumerateVerticesChecked(
                graph, sources, frontier, exploredSet);
    }
}
