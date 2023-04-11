#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Collections.Generic;
    using Edge = Endpoints<int>;

    public static class Int32FrozenAdjacencyGraphFactory
    {
#if NET5_0_OR_GREATER
        public static Int32FrozenAdjacencyGraph FromEdgeList(Span<Edge> edges, int vertexCount)
        {
            if (vertexCount is 0)
                return default;
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            edges.Sort(EdgeComparer.Instance);
            return FromSortedEdgeList(edges, vertexCount);
        }
#endif

        public static Int32FrozenAdjacencyGraph FromEdgeList(Edge[] edges, int vertexCount)
        {
            if (vertexCount is 0)
                return default;
            if (edges is null)
                ThrowHelper.ThrowArgumentNullException(nameof(edges));
            if (vertexCount < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(vertexCount));

            Array.Sort(edges, EdgeComparer.Instance);
            return FromSortedEdgeList(edges, vertexCount);
        }

        private static Int32FrozenAdjacencyGraph FromSortedEdgeList(
            ReadOnlySpan<Edge> edgesSortedByTail, int vertexCount) => throw new NotImplementedException();

        private sealed class EdgeComparer : IComparer<Edge>
        {
            internal static EdgeComparer Instance { get; } = new();

            public int Compare(Edge x, Edge y) => x.Tail.CompareTo(y.Tail);
        }
    }
}
#endif
