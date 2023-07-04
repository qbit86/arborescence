namespace Arborescence.Traversal.Adjacency
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static partial class EagerBfs<TVertex, TVertexEnumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, TVertex source, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<TVertex, TVertexEnumerator>
            where THandler : IBfsHandler<TVertex, Endpoints<TVertex>, TGraph> =>
            throw new NotImplementedException();
    }
}
