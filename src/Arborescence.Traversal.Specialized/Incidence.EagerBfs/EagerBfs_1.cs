namespace Arborescence.Traversal.Specialized.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class EagerBfs<TEdge>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where THandler : IBfsHandler<int, TEdge, TGraph> =>
            EagerBfs<TEdge, IEnumerator<TEdge>>.TraverseChecked(
                graph, source, vertexCount, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<int>
            where THandler : IBfsHandler<int, TEdge, TGraph> =>
            EagerBfs<TEdge, IEnumerator<TEdge>>.TraverseChecked(
                graph, sources, vertexCount, handler, cancellationToken);
    }
}
