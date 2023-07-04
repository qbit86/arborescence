namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static partial class EagerBfs<TVertex>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, TVertex source, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            EagerBfs<TVertex, IEnumerator<TVertex>>.TraverseChecked(graph, source, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            EagerBfs<TVertex, IEnumerator<TVertex>>.TraverseChecked(
                graph, source, comparer, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, IEnumerator<TVertex>>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            EagerBfs<TVertex, IEnumerator<TVertex>>.TraverseChecked(
                graph, source, colorByVertex, handler, cancellationToken);
    }
}
