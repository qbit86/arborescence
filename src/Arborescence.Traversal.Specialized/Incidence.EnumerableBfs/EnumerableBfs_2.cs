namespace Arborescence.Traversal.Specialized.Incidence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableBfs<TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator> =>
            EnumerateVerticesChecked(graph, source, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph>(
            TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator> =>
            EnumerateEdgesChecked(graph, source, vertexCount);

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableBfs<int, TEdge, TEdgeEnumerator>
                    .EnumerateVerticesIterator(graph, source, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<TEdge> EnumerateEdgesChecked<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Incidence.EnumerableBfs<int, TEdge, TEdgeEnumerator>
                    .EnumerateEdgesIterator(graph, source, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
