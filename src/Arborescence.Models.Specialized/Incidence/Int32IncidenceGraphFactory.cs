namespace Arborescence.Models.Specialized
{
    using System;
    using System.Diagnostics;
#if NET5_0_OR_GREATER
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
#endif

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32IncidenceGraph"/> type.
    /// </summary>
    public static class Int32IncidenceGraphFactory
    {
        /// <summary>
        /// Creates an <see cref="Int32IncidenceGraph"/> with the specified edges defined by their endpoint pairs.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method sorts the incoming collection of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="endpointsByEdge">
        /// The collection that maps an edge in a range [0, edgeCount) to a pair of its endpoints.
        /// </param>
        /// <returns>
        /// An <see cref="Int32IncidenceGraph"/> containing the specified edges
        /// defined by their endpoint pairs.
        /// </returns>
        public static Int32IncidenceGraph FromEdges(Endpoints<int>[] endpointsByEdge)
        {
            if (endpointsByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(endpointsByEdge));

            int vertexCount = DeduceVertexCount(endpointsByEdge);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromEdgesUnchecked(vertexCount, endpointsByEdge);
        }

        /// <summary>
        /// Creates an <see cref="Int32IncidenceGraph"/> with the specified edges defined by their endpoint pairs
        /// and number of vertices.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method sorts the incoming collection of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="endpointsByEdge">
        /// The collection that maps an edge in a range [0, edgeCount) to a pair of its endpoints.
        /// </param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>
        /// An <see cref="Int32IncidenceGraph"/> containing the specified edges
        /// and the vertices in the range [0, <paramref name="vertexCount"/>).
        /// </returns>
        public static Int32IncidenceGraph FromEdges(Endpoints<int>[] endpointsByEdge, int vertexCount)
        {
            if (endpointsByEdge is null)
                ArgumentNullExceptionHelpers.Throw(nameof(endpointsByEdge));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            // We cannot reuse this overload taking span:
            // Int32IncidenceGraphFactory.FromEdgesUnchecked(int, Span<Endpoints<int>>)
            // Because the sorting algorithm that takes spans is only available in .NET 5 and later:
            // MemoryExtensions.Sort<TKey,TValue,TComparer>(Span<TKey>, Span<TValue>, TComparer)
            // https://learn.microsoft.com/en-us/dotnet/api/system.memoryextensions.sort?view=net-6.0

            // We need to implement sorting independently with another overload that takes arrays:
            // Int32IncidenceGraphFactory.FromEdgesUnchecked(int, Endpoints<int>[])
            // Because the sorting algorithm that takes arrays is available on older versions of .NET:
            // Array.Sort<TKey,TValue>(TKey[], TValue[], IComparer<TKey>)
            // https://learn.microsoft.com/en-us/dotnet/api/system.array.sort?view=net-6.0

            return FromEdgesUnchecked(vertexCount, endpointsByEdge);
        }

        private static int DeduceVertexCount(ReadOnlySpan<Endpoints<int>> endpointsByEdge)
        {
            if (endpointsByEdge.Length is 0)
                return 0;

            int maxVertex = -1;
            foreach (Endpoints<int> endpoints in endpointsByEdge)
                maxVertex = Math.Max(maxVertex, Math.Max(endpoints.Tail, endpoints.Head));

            return maxVertex + 1;
        }

        private static Int32IncidenceGraph CreateTrivial(int vertexCount)
        {
            int dataLength = 2 + vertexCount;
            int[] data = ArrayHelpers.AllocateUninitializedArray<int>(dataLength);
            data[0] = vertexCount;
            data[1] = 0;
            Array.Fill(data, dataLength, 2, vertexCount);
            return new(data);
        }

        private static void PopulateUpperBoundByVertex(
            int vertexCount, int edgeCount, ReadOnlySpan<int> edgesOrderedByTail, ReadOnlySpan<int> tailByEdge,
            Span<int> upperBoundByVertex)
        {
            int offset = 2 + vertexCount;
            for (int lower = 0, expectedTail = 0; lower < edgeCount;)
            {
                int actualTail = tailByEdge[edgesOrderedByTail[lower]];
                if (actualTail >= vertexCount)
                    break;

                if (expectedTail < actualTail)
                {
                    int filler = expectedTail is 0 ? offset : upperBoundByVertex[expectedTail - 1];
                    int length = Math.Clamp(actualTail - expectedTail, 0, vertexCount);
                    upperBoundByVertex.Slice(expectedTail, length).Fill(filler);
                }

                int upper = lower + 1;
                for (; upper < edgeCount; ++upper)
                {
                    if (tailByEdge[edgesOrderedByTail[upper]] > actualTail)
                        break;
                }

                upperBoundByVertex[actualTail] = offset + upper;
                lower = upper;
                expectedTail = actualTail + 1;
            }
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Creates an <see cref="Int32IncidenceGraph"/> with the specified edges defined by their endpoint pairs.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method sorts the incoming collection of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="endpointsByEdge">
        /// The collection that maps an edge in a range [0, edgeCount) to a pair of its endpoints.
        /// </param>
        /// <returns>
        /// An <see cref="Int32IncidenceGraph"/> containing the specified edges
        /// defined by their endpoint pairs.
        /// </returns>
        public static Int32IncidenceGraph FromEdges(Span<Endpoints<int>> endpointsByEdge)
        {
            int vertexCount = DeduceVertexCount(endpointsByEdge);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromEdgesUnchecked(vertexCount, endpointsByEdge);
        }

        /// <summary>
        /// Creates an <see cref="Int32IncidenceGraph"/> with the specified edges defined by their endpoint pairs
        /// and number of vertices.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method sorts the incoming collection of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="endpointsByEdge">
        /// The collection that maps an edge in a range [0, edgeCount) to a pair of its endpoints.
        /// </param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>
        /// An <see cref="Int32IncidenceGraph"/> containing the specified edges
        /// and the vertices in the range [0, <paramref name="vertexCount"/>).
        /// </returns>
        public static Int32IncidenceGraph FromEdges(Span<Endpoints<int>> endpointsByEdge, int vertexCount)
        {
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            return FromEdgesUnchecked(vertexCount, endpointsByEdge);
        }

        /// <summary>
        /// Creates an <see cref="Int32IncidenceGraph"/> with the specified edges defined by their endpoint pairs.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method sorts the incoming collection of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="endpointsByEdge">
        /// The collection that maps an edge in a range [0, edgeCount) to a pair of its endpoints.
        /// </param>
        /// <returns>
        /// An <see cref="Int32IncidenceGraph"/> containing the specified edges
        /// defined by their endpoint pairs.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="endpointsByEdge"/> is <see langword="null"/>.
        /// </exception>
        public static Int32IncidenceGraph FromEdges(List<Endpoints<int>> endpointsByEdge)
        {
            if (endpointsByEdge is null)
                throw new ArgumentNullException(nameof(endpointsByEdge));

            Span<Endpoints<int>> endpointsByEdgeSpan = CollectionsMarshal.AsSpan(endpointsByEdge);
            int vertexCount = DeduceVertexCount(endpointsByEdgeSpan);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromEdgesUnchecked(vertexCount, endpointsByEdgeSpan);
        }

        /// <summary>
        /// Creates an <see cref="Int32IncidenceGraph"/> with the specified edges defined by their endpoint pairs
        /// and number of vertices.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method sorts the incoming collection of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="endpointsByEdge">
        /// The collection that maps an edge in a range [0, edgeCount) to a pair of its endpoints.
        /// </param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>
        /// An <see cref="Int32IncidenceGraph"/> containing the specified edges
        /// and the vertices in the range [0, <paramref name="vertexCount"/>).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="endpointsByEdge"/> is <see langword="null"/>.
        /// </exception>
        public static Int32IncidenceGraph FromEdges(List<Endpoints<int>> endpointsByEdge, int vertexCount)
        {
            if (endpointsByEdge is null)
                throw new ArgumentNullException(nameof(endpointsByEdge));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            return FromEdgesUnchecked(vertexCount, CollectionsMarshal.AsSpan(endpointsByEdge));
        }

        private static Int32IncidenceGraph FromEdgesUnchecked(
            int vertexCount, Span<Endpoints<int>> endpointsByEdge)
        {
            int edgeCount = endpointsByEdge.Length;
            if (edgeCount is 0)
                return CreateTrivial(vertexCount);

            int dataLength = 2 + vertexCount + edgeCount * 3;
            int[] data = ArrayHelpers.AllocateUninitializedArray<int>(dataLength);
            data[0] = vertexCount;
            data[1] = edgeCount;
            Span<int> edgesOrderedByTail = data.AsSpan(2 + vertexCount, edgeCount);
            Span<int> headByEdge = data.AsSpan(2 + vertexCount + edgeCount, edgeCount);
            Span<int> tailByEdge = data.AsSpan(2 + vertexCount + edgeCount + edgeCount, edgeCount);
            for (int i = 0; i < edgeCount; ++i)
            {
                edgesOrderedByTail[i] = i;
                Endpoints<int> endpoints = endpointsByEdge[i];
                headByEdge[i] = endpoints.Head;
                tailByEdge[i] = endpoints.Tail;
            }

            endpointsByEdge.Sort(edgesOrderedByTail, EdgeComparer.Instance);

            Span<int> upperBoundByVertex = data.AsSpan(2, vertexCount);
            PopulateUpperBoundByVertex(vertexCount, edgeCount, edgesOrderedByTail, tailByEdge, upperBoundByVertex);

            return new(data);
        }
#else
        private static Int32IncidenceGraph FromEdgesUnchecked(int vertexCount, Endpoints<int>[] endpointsByEdge)
        {
            int edgeCount = endpointsByEdge.Length;
            if (edgeCount is 0)
                return CreateTrivial(vertexCount);

            int dataLength = 2 + vertexCount + edgeCount * 3;
            int[] data = ArrayHelpers.AllocateUninitializedArray<int>(dataLength);
            data[0] = vertexCount;
            data[1] = edgeCount;
            int[] edgesOrderedByTailBuffer = System.Buffers.ArrayPool<int>.Shared.Rent(edgeCount);
            Span<int> headByEdge = data.AsSpan(2 + vertexCount + edgeCount, edgeCount);
            Span<int> tailByEdge = data.AsSpan(2 + vertexCount + edgeCount + edgeCount, edgeCount);
            for (int i = 0; i < edgeCount; ++i)
            {
                edgesOrderedByTailBuffer[i] = i;
                Endpoints<int> endpoints = endpointsByEdge[i];
                headByEdge[i] = endpoints.Head;
                tailByEdge[i] = endpoints.Tail;
            }

            Array.Sort(endpointsByEdge, edgesOrderedByTailBuffer, EdgeComparer.Instance);
            Span<int> edgesOrderedByTail = data.AsSpan(2 + vertexCount, edgeCount);
            edgesOrderedByTailBuffer.AsSpan(0, edgeCount).CopyTo(edgesOrderedByTail);
            System.Buffers.ArrayPool<int>.Shared.Return(edgesOrderedByTailBuffer);

            Span<int> upperBoundByVertex = data.AsSpan(2, vertexCount);
            PopulateUpperBoundByVertex(vertexCount, edgeCount, edgesOrderedByTail, tailByEdge, upperBoundByVertex);

            return new(data);
        }
#endif
    }
}
