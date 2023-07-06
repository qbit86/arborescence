namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class EagerBfs<TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<int>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Traverse<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken = default)
            where TGraph : IOutNeighborsAdjacency<int, TVertexEnumerator>
            where THandler : IBfsHandler<int, Endpoints<int>, TGraph> =>
            TraverseChecked(graph, source, vertexCount, handler, cancellationToken);

        internal static void TraverseChecked<TGraph, THandler>(
            TGraph graph, int source, int vertexCount, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IOutNeighborsAdjacency<int, TVertexEnumerator>
            where THandler : IBfsHandler<int, Endpoints<int>, TGraph>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (handler is null)
                ArgumentNullExceptionHelpers.Throw(nameof(handler));

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            try
            {
                var colorByVertex = new Int32ColorDictionary(arrayFromPool);
                Arborescence.Traversal.Adjacency.EagerBfs<int, TVertexEnumerator>.TraverseUnchecked(
                    graph, source, colorByVertex, handler, cancellationToken);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
