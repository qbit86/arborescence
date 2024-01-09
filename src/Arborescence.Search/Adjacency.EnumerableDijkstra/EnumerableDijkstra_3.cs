namespace Arborescence.Search.Adjacency
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents Dijkstra's algorithm for solving the single source shortest paths problem.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TNeighborEnumerator">The type of the neighbor enumerator.</typeparam>
    /// <typeparam name="TWeight">The type of edge weight.</typeparam>
    public static partial class EnumerableDijkstra<TVertex, TNeighborEnumerator, TWeight>
        where TVertex : notnull
        where TNeighborEnumerator : IEnumerator<TVertex> { }
}
