namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableBfs<TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<int>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator> =>
            EnumerateVerticesChecked(graph, source, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<int> =>
            EnumerateVerticesChecked(graph, sources, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph>(
            TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator> =>
            EnumerateEdgesChecked(graph, source, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<int> =>
            EnumerateEdgesChecked(graph, sources, vertexCount);

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph>(
            TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Adjacency.EnumerableBfs<int, TNeighborEnumerator>
                    .EnumerateVerticesIterator(graph, source, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<int> EnumerateVerticesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Adjacency.EnumerableBfs<int, TNeighborEnumerator>
                    .EnumerateVerticesIterator(graph, sources, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<Endpoints<int>> EnumerateEdgesChecked<TGraph>(
            TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Adjacency.EnumerableBfs<int, TNeighborEnumerator>
                    .EnumerateEdgesIterator(graph, source, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }

        internal static IEnumerable<Endpoints<int>> EnumerateEdgesChecked<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<int>
        {
            if (graph is null)
                ArgumentNullExceptionHelpers.Throw(nameof(graph));

            if (sources is null)
                ArgumentNullExceptionHelpers.Throw(nameof(sources));

            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.ThrowNegative(nameof(vertexCount), vertexCount);

            byte[] arrayFromPool = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(arrayFromPool, 0, vertexCount);
            try
            {
                Int32Set exploredSet = new(arrayFromPool);
                return Arborescence.Traversal.Adjacency.EnumerableBfs<int, TNeighborEnumerator>
                    .EnumerateEdgesIterator(graph, sources, exploredSet);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(arrayFromPool);
            }
        }
    }
}
