namespace Arborescence.Search.Incidence
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents Dijkstra's algorithm for solving the single source shortest paths problem.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    /// <typeparam name="TWeight">The type of edge weight.</typeparam>
    public static partial class EnumerableDijkstra<TVertex, TEdge, TEdgeEnumerator, TWeight>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
