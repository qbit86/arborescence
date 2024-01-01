namespace Arborescence.Models.Specialized
{
#if NET5_0_OR_GREATER
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
#endif
    using System;
    using System.Diagnostics;
    using Edge = Endpoints<int>;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32AdjacencyGraph"/> type.
    /// </summary>
    public static class Int32AdjacencyGraphFactory
    {
        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified edges.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method may sort the incoming sequence of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="edges">The edges of the graph.</param>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> containing the specified edges.
        /// </returns>
        public static Int32AdjacencyGraph FromEdges(Edge[] edges)
        {
            if (edges is null)
                ArgumentNullExceptionHelpers.Throw(nameof(edges));

            if (ShouldOrderByTail(edges, out int vertexCount))
                Array.Sort(edges, EdgeComparer.Instance);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromOrderedEdges(vertexCount, edges);
        }

        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified edges and number of vertices.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method may sort the incoming sequence of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="edges">The edges of the graph.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> containing the specified edges
        /// and the vertices in the range [0, <paramref name="vertexCount"/>).
        /// </returns>
        public static Int32AdjacencyGraph FromEdges(Edge[] edges, int vertexCount)
        {
            if (edges is null)
                ArgumentNullExceptionHelpers.Throw(nameof(edges));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(vertexCount));

            Array.Sort(edges, EdgeComparer.Instance);
            return FromOrderedEdges(vertexCount, edges);
        }

        private static bool ShouldOrderByTail(ReadOnlySpan<Edge> edges, out int vertexCount)
        {
            if (edges.Length is 0)
                return TryHelpers.None(out vertexCount);

            bool shouldOrder = false;
            int maxTail = edges[0].Tail;
            int maxVertex = Math.Max(maxTail, edges[0].Head);
            ReadOnlySpan<Edge> remainingEdges = edges[1..];
            foreach (Endpoints<int> edge in remainingEdges)
            {
                if (edge.Tail < maxTail)
                {
                    shouldOrder = true;
                    maxVertex = Math.Max(maxVertex, edge.Head);
                }
                else
                {
                    maxTail = edge.Tail;
                    maxVertex = Math.Max(maxVertex, Math.Max(maxTail, edge.Head));
                }
            }

            vertexCount = maxVertex + 1;
            return shouldOrder;
        }

        private static Int32AdjacencyGraph FromOrderedEdges(
            int vertexCount, ReadOnlySpan<Edge> edgesOrderedByTail)
        {
            int edgeCount = edgesOrderedByTail.Length;
            if (edgeCount is 0)
                return CreateTrivial(vertexCount);

            int dataLength = 2 + vertexCount + edgeCount;
            int[] data = ArrayHelpers.AllocateUninitializedArray<int>(dataLength);
            data[0] = vertexCount;
            data[1] = edgeCount;
            Span<int> neighborsOrderedByTail = data.AsSpan(2 + vertexCount, edgeCount);
            for (int i = 0; i < edgeCount; ++i)
                neighborsOrderedByTail[i] = edgesOrderedByTail[i].Head;

            Span<int> upperBoundByVertex = data.AsSpan(2, vertexCount);
            int offset = 2 + vertexCount;
            for (int lower = 0, expectedTail = 0; lower < edgeCount;)
            {
                int actualTail = edgesOrderedByTail[lower].Tail;
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
                    if (edgesOrderedByTail[upper].Tail != actualTail)
                    {
                        Debug.Assert(edgesOrderedByTail[upper].Tail > actualTail);
                        break;
                    }
                }

                upperBoundByVertex[actualTail] = offset + upper;
                lower = upper;
                expectedTail = actualTail + 1;
            }

            return new(data);
        }

        private static Int32AdjacencyGraph CreateTrivial(int vertexCount)
        {
            int dataLength = 2 + vertexCount;
            int[] data = ArrayHelpers.AllocateUninitializedArray<int>(dataLength);
            data[0] = vertexCount;
            data[1] = 0;
            Array.Fill(data, dataLength, 2, vertexCount);
            return new(data);
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified edges.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method may sort the incoming sequence of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="edges">The edges of the graph.</param>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> containing the specified edges.
        /// </returns>
        public static Int32AdjacencyGraph FromEdges(Span<Edge> edges)
        {
            if (ShouldOrderByTail(edges, out int vertexCount))
                edges.Sort(EdgeComparer.Instance);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromOrderedEdges(vertexCount, edges);
        }

        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified edges and number of vertices.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method may sort the incoming sequence of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="edges">The edges of the graph.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> containing the specified edges
        /// and the vertices in the range [0, <paramref name="vertexCount"/>).
        /// </returns>
        public static Int32AdjacencyGraph FromEdges(Span<Edge> edges, int vertexCount)
        {
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(vertexCount));

            edges.Sort(EdgeComparer.Instance);
            return FromOrderedEdges(vertexCount, edges);
        }

        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified edges.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method may sort the incoming sequence of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="edges">The edges of the graph.</param>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> containing the specified edges.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="edges"/> is <see langword="null"/>.
        /// </exception>
        public static Int32AdjacencyGraph FromEdges(List<Edge> edges)
        {
            if (edges is null)
                ArgumentNullExceptionHelpers.Throw(nameof(edges));

            Span<Edge> edgeSpan = CollectionsMarshal.AsSpan(edges);
#pragma warning disable CA1062
            if (ShouldOrderByTail(edgeSpan, out int vertexCount))
                edges.Sort(EdgeComparer.Instance);
#pragma warning restore CA1062
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromOrderedEdges(vertexCount, edgeSpan);
        }

        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified edges and number of vertices.
        /// </summary>
        /// <remarks>
        /// As a side effect, this method may sort the incoming sequence of edges,
        /// which entails an O(n·log n) operation.
        /// </remarks>
        /// <param name="edges">The edges of the graph.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> containing the specified edges
        /// and the vertices in the range [0, <paramref name="vertexCount"/>).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="edges"/> is <see langword="null"/>.
        /// </exception>
        public static Int32AdjacencyGraph FromEdges(List<Edge> edges, int vertexCount)
        {
            if (edges is null)
                ArgumentNullExceptionHelpers.Throw(nameof(edges));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ArgumentOutOfRangeExceptionHelpers.Throw(nameof(vertexCount));

#pragma warning disable CA1062
            edges.Sort(EdgeComparer.Instance);
#pragma warning restore CA1062
            return FromOrderedEdges(vertexCount, CollectionsMarshal.AsSpan(edges));
        }
#endif
    }
}
