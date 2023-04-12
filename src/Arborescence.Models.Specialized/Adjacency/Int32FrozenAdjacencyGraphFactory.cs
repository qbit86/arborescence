#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
#if NET5_0_OR_GREATER
    using System.Runtime.InteropServices;
#endif
    using System;
    using System.Collections.Generic;
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
            return FromSortedEdges(edges, vertexCount);
        }

        private static Int32FrozenAdjacencyGraph FromSortedEdges(
            ReadOnlySpan<Edge> edgesSortedByTail, int vertexCount) => throw new NotImplementedException();

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
            return FromSortedEdges(edges, vertexCount);
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
            return FromSortedEdges(CollectionsMarshal.AsSpan(edges), vertexCount);
        }
#endif
    }
}
#endif
