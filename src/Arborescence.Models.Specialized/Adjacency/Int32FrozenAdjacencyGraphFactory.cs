#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
#if NET5_0_OR_GREATER
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
#endif
    using System;
    using System.Diagnostics;
    using Edge = Endpoints<int>;

    public static class Int32FrozenAdjacencyGraphFactory
    {
        public static Int32FrozenAdjacencyGraph FromEdges(Edge[] edges)
        {
            if (edges is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edges));

            Array.Sort(edges, EdgeComparer.Instance);
            int vertexCount = DeduceVertexCount(edges);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromOrderedEdges(vertexCount, edges);
        }

        public static Int32FrozenAdjacencyGraph FromEdges(int vertexCount, Edge[] edges)
        {
            if (edges is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edges));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            Array.Sort(edges, EdgeComparer.Instance);
            return FromOrderedEdges(vertexCount, edges);
        }

        private static int DeduceVertexCount(ReadOnlySpan<Edge> edges)
        {
            int maxVertex = -1;
            foreach (Endpoints<int> edge in edges)
            {
                int vertex = Math.Max(edge.Tail, edge.Head);
                maxVertex = Math.Max(maxVertex, vertex);
            }

            return maxVertex + 1;
        }

        private static Int32FrozenAdjacencyGraph FromOrderedEdges(
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
                    int filler = expectedTail is 0 ? 0 : upperBoundByVertex[expectedTail - 1];
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

        private static Int32FrozenAdjacencyGraph CreateTrivial(int vertexCount)
        {
            int dataLength = 2 + vertexCount;
            int[] data = ArrayHelpers.AllocateUninitializedArray<int>(dataLength);
            data[0] = vertexCount;
            data[1] = 0;
            Array.Fill(data, dataLength, 2, vertexCount);
            return new(data);
        }

#if NET5_0_OR_GREATER
        public static Int32FrozenAdjacencyGraph FromEdges(Span<Edge> edges)
        {
            edges.Sort(EdgeComparer.Instance);
            int vertexCount = DeduceVertexCount(edges);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromOrderedEdges(vertexCount, edges);
        }

        public static Int32FrozenAdjacencyGraph FromEdges(int vertexCount, Span<Edge> edges)
        {
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            edges.Sort(EdgeComparer.Instance);
            return FromOrderedEdges(vertexCount, edges);
        }

        public static Int32FrozenAdjacencyGraph FromEdges(List<Edge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            edges.Sort(EdgeComparer.Instance);
            Span<Edge> edgeSpan = CollectionsMarshal.AsSpan(edges);
            int vertexCount = DeduceVertexCount(edgeSpan);
            if (vertexCount is 0)
                return default;
            Debug.Assert(vertexCount > 0);

            return FromOrderedEdges(vertexCount, edgeSpan);
        }

        public static Int32FrozenAdjacencyGraph FromEdges(int vertexCount, List<Edge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            edges.Sort(EdgeComparer.Instance);
            return FromOrderedEdges(vertexCount, CollectionsMarshal.AsSpan(edges));
        }
#endif
    }
}
#endif
