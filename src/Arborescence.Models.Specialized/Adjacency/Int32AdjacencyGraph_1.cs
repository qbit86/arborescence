namespace Arborescence.Models.Specialized
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32AdjacencyGraph"/> type.
    /// </summary>
    /// <typeparam name="TNeighborCollection">The type of the neighbor collection.</typeparam>
    public static class Int32AdjacencyGraph<TNeighborCollection>
        where TNeighborCollection : ICollection<int>
    {
        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified list of neighbor lists.
        /// </summary>
        /// <param name="neighborsByVertex">
        /// The list that maps a vertex in a range [0, vertexCount) to a list of its neighbors.
        /// </param>
        /// <typeparam name="TMultimap">
        /// The type of the list that maps a vertex in a range [0, vertexCount) to a list of its neighbors.
        /// </typeparam>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> defined by the lists of neighbors.
        /// </returns>
        public static Int32AdjacencyGraph FromList<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyList<TNeighborCollection> =>
            neighborsByVertex is null ? default : FromListUnchecked(neighborsByVertex);

        /// <summary>
        /// Creates an <see cref="Int32AdjacencyGraph"/> with the specified dictionary of neighbor lists.
        /// </summary>
        /// <param name="neighborsByVertex">
        /// The dictionary that maps a vertex to a list of its neighbors.
        /// </param>
        /// <typeparam name="TMultimap">
        /// The type of the dictionary that maps a vertex to a list of its neighbors.
        /// </typeparam>
        /// <returns>
        /// An <see cref="Int32AdjacencyGraph"/> defined by the dictionary that maps a vertex to a list of its neighbors.
        /// </returns>
        public static Int32AdjacencyGraph FromDictionary<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyDictionary<int, TNeighborCollection> =>
            neighborsByVertex is null ? default : FromDictionaryUnchecked(neighborsByVertex);

        private static Int32AdjacencyGraph FromListUnchecked<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyList<TNeighborCollection>
        {
            var dictionary = Int32ReadOnlyDictionary<TNeighborCollection>.Create(neighborsByVertex);
            return FromDictionaryUnchecked(dictionary);
        }

        private static Int32AdjacencyGraph FromDictionaryUnchecked<TMultimap>(TMultimap neighborsByVertex)
            where TMultimap : IReadOnlyDictionary<int, TNeighborCollection>
        {
            int vertexCount = neighborsByVertex.Count;
            ArraySegment<int> data = new();
            data = ArraySegmentBuilder.Add(data, vertexCount, false);
            data = ArraySegmentBuilder.Add(data, 0, false);
            data = ArraySegmentBuilder.EnsureSize(data, data.Count + vertexCount, false);
            var upperBoundByVertex = data.Array.AsSpan(2, vertexCount);
            int offset = 2 + vertexCount;
            for (int key = 0; key < vertexCount; ++key)
            {
                if (!neighborsByVertex.TryGetValue(key, out var neighbors) || neighbors is null)
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

                data = ArraySegmentBuilder.EnsureSize(data, data.Count + neighborCount, false);
                neighbors.CopyTo(data.Array!, offset);
                offset += neighborCount;
                upperBoundByVertex[key] = offset;
            }

            return new(data.Array!);
        }
    }
}
