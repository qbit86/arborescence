namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the BFS algorithm — breadth-first traversal of the graph — implemented as enumerator.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    public static partial class EnumerableBfs<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex> { }
}
