namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the BFS algorithm — breadth-first traversal of the graph — implemented as iterator.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static partial class EnumerableBfs<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
