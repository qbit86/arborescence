#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Arborescence.Models.Specialized
{
    using System;
    using System.Collections.Generic;

    public static class Int32FrozenAdjacencyGraphFactory<TVertexCollection, TVertexEnumerator>
        where TVertexCollection : ICollection<int>
    {
        public static Int32FrozenAdjacencyGraph Create<TMultimap, TVertexCollectionPolicy>(
            TMultimap neighborsByVertex, TVertexCollectionPolicy collectionPolicy)
            where TMultimap : IReadOnlyDictionary<int, TVertexCollection>
            where TVertexCollectionPolicy : IEnumerablePolicy<TVertexCollection, TVertexEnumerator>
        {
            if (neighborsByVertex is null)
                return default;

            int vertexCount = neighborsByVertex.Count;
            ArrayPrefix<int> data = new();
            data = ArrayPrefixBuilder.Add(data, vertexCount, false);
            data = ArrayPrefixBuilder.Add(data, 0, false);
            data = ArrayPrefixBuilder.EnsureSize(data, data.Count + vertexCount, false);
            Span<int> upperBoundByVertex = data.Array.AsSpan(2, vertexCount);
            int offset = 2 + vertexCount;
            for (int key = 0; key < vertexCount; ++key)
            {
                if (!neighborsByVertex.TryGetValue(key, out TVertexCollection? neighbors))
                    continue;

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
#endif
