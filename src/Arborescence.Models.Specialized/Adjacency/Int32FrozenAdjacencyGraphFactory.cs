#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Collections.Generic;
    using Edge = Endpoints<int>;

    public static class Int32FrozenAdjacencyGraphFactory<TVertexCollection>
        where TVertexCollection : ICollection<int>
    {
        public static Int32FrozenAdjacencyGraph FromList<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyList<TVertexCollection> =>
            neighborsByVertex is null ? default : FromListUnchecked(neighborsByVertex);

        public static Int32FrozenAdjacencyGraph FromDictionary<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyDictionary<int, TVertexCollection> =>
            neighborsByVertex is null ? default : FromDictionaryUnchecked(neighborsByVertex);

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

        private static Int32FrozenAdjacencyGraph FromListUnchecked<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyList<TVertexCollection>
        {
            Int32ReadOnlyDictionary<TVertexCollection, TMultimap> dictionary =
                Int32ReadOnlyDictionaryFactory<TVertexCollection>.Create(neighborsByVertex);
            return FromDictionaryUnchecked(dictionary);
        }

        private static Int32FrozenAdjacencyGraph FromDictionaryUnchecked<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyDictionary<int, TVertexCollection>
        {
            int vertexCount = neighborsByVertex.Count;
            ArrayPrefix<int> data = new();
            data = ArrayPrefixBuilder.Add(data, vertexCount, false);
            data = ArrayPrefixBuilder.Add(data, 0, false);
            data = ArrayPrefixBuilder.EnsureSize(data, data.Count + vertexCount, false);
            Span<int> upperBoundByVertex = data.Array.AsSpan(2, vertexCount);
            int offset = 2 + vertexCount;
            for (int key = 0; key < vertexCount; ++key)
            {
                if (!neighborsByVertex.TryGetValue(key, out TVertexCollection? neighbors) || neighbors is null)
                {
                    upperBoundByVertex[key] = offset;
                    continue;
                }

                int neighborCount = neighbors.Count;
                if (neighborCount is 0)
                {
                    upperBoundByVertex[key] = offset;
                    continue;
                }

                data = ArrayPrefixBuilder.EnsureSize(data, data.Count + neighborCount, false);
                neighbors.CopyTo(data.Array!, offset);
                offset += neighborCount;
                upperBoundByVertex[key] = offset;
            }

            return new(data.Array!);
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
