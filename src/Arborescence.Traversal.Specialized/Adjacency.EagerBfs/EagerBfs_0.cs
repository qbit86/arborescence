namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class EagerBfs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where THandler : IBfsHandler<int, Endpoints<int>, TGraph> =>
            EagerBfs<IEnumerator<int>>.TraverseChecked(graph, source, vertexCount, handler, cancellationToken);
    }
}
