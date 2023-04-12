#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
#if NET5_0_OR_GREATER
    using System.Runtime.InteropServices;
#endif
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Edge = Endpoints<int>;

    public static class Int32FrozenAdjacencyGraphFactory
    {
        public static Int32FrozenAdjacencyGraph FromEdges(Edge[] edges, int vertexCount)
        {
            if (edges is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edges));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            Array.Sort(edges, EdgeComparer.Instance);
            return FromOrderedEdges(edges, vertexCount);
        }

        private static Int32FrozenAdjacencyGraph FromOrderedEdges(
            ReadOnlySpan<Edge> edgesOrderedByTail, int vertexCount)
        {
            int edgeCount = edgesOrderedByTail.Length;
            if (edgeCount is 0)
                return CreateTrivial(vertexCount);

            int dataLength = 2 + vertexCount + edgeCount;
#if NET5_0_OR_GREATER
            int[] data = GC.AllocateUninitializedArray<int>(dataLength);
#else
            int[] data = new int[dataLength];
#endif
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
#if NET5_0_OR_GREATER
            int[] data = GC.AllocateUninitializedArray<int>(dataLength);
#else
            int[] data = new int[dataLength];
#endif
            data[0] = vertexCount;
            data[1] = 0;
            Array.Fill(data, dataLength, 2, vertexCount);
            return new(data);
        }

        private sealed class EdgeComparer : IComparer<Edge>
        {
            internal static EdgeComparer Instance { get; } = new();

            public int Compare(Edge x, Edge y) => x.Tail.CompareTo(y.Tail);
        }

#if NET5_0_OR_GREATER
        public static Int32FrozenAdjacencyGraph FromEdges(Span<Edge> edges, int vertexCount)
        {
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            edges.Sort(EdgeComparer.Instance);
            return FromOrderedEdges(edges, vertexCount);
        }

        public static Int32FrozenAdjacencyGraph FromEdges(List<Edge> edges, int vertexCount)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            edges.Sort(EdgeComparer.Instance);
            return FromOrderedEdges(CollectionsMarshal.AsSpan(edges), vertexCount);
        }
#endif
    }
}
#endif
