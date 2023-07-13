namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class EagerDfs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where THandler : IDfsHandler<int, Endpoints<int>, TGraph> =>
            EagerDfs<IEnumerator<int>>.TraverseChecked(graph, source, vertexCount, handler, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, TSourceCollection, THandler>(
            TGraph graph, TSourceCollection sources, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where TSourceCollection : IEnumerable<int>
            where THandler : IDfsHandler<int, Endpoints<int>, TGraph> =>
            EagerDfs<IEnumerator<int>>.TraverseChecked(graph, sources, vertexCount, handler, cancellationToken);
    }
}