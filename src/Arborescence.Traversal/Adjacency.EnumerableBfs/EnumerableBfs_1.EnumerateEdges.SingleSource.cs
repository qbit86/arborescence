#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET47_OR_GREATER
namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs<TVertex>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph>(
            TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(graph, source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(graph, source, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<(TVertex Tail, TVertex Head)> EnumerateEdges<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateEdgesChecked(graph, source, exploredSet);
    }
}
#endif
