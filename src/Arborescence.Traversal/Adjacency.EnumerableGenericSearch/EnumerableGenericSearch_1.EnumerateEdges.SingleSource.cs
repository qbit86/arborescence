#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET47_OR_GREATER
namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableGenericSearch<TVertex>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(
                graph, source, frontier);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, TVertex source, TFrontier frontier, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TFrontier : IProducerConsumerCollection<TVertex> =>
            EnumerableGenericSearch<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(
                graph, source, frontier, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableGenericSearch<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(
                graph, source, frontier, exploredSet);
    }
}
#endif
