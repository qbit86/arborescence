namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the generic search algorithm — traversal of the graph
    /// where the order of exploring vertices is determined by the frontier implementation.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex> { }
}
