namespace Arborescence.Models.Specialized
{
    using System;
    using System.Collections.Generic;

    public static class Int32AdjacencyGraphFactory<TVertexCollection>
        where TVertexCollection : ICollection<int>
    {
        public static Int32AdjacencyGraph FromList<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyList<TVertexCollection> =>
            neighborsByVertex is null ? default : FromListUnchecked(neighborsByVertex);

        public static Int32AdjacencyGraph FromDictionary<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyDictionary<int, TVertexCollection> =>
            neighborsByVertex is null ? default : FromDictionaryUnchecked(neighborsByVertex);

        private static Int32AdjacencyGraph FromListUnchecked<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyList<TVertexCollection>
        {
            Int32ReadOnlyDictionary<TVertexCollection, TMultimap> dictionary =
                Int32ReadOnlyDictionaryFactory<TVertexCollection>.Create(neighborsByVertex);
            return FromDictionaryUnchecked(dictionary);
        }

        private static Int32AdjacencyGraph FromDictionaryUnchecked<TMultimap>(TMultimap neighborsByVertex)
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
    }
}
