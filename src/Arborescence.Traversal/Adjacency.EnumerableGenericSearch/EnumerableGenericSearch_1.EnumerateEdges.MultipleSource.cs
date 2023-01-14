#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET47_OR_GREATER
namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableGenericSearch<TVertex>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<
            TGraph, TSourceEnumerator, TFrontier>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(graph, sources, frontier);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<
            TGraph, TSourceEnumerator, TFrontier>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(
                graph, sources, frontier, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<
            TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableGenericSearch<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(
                graph, sources, frontier, exploredSet);
    }
}
#endif
