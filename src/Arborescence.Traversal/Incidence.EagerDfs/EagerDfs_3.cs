namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a non-recursive manner.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static partial class EagerDfs<TVertex, TEdge, TEdgeEnumerator>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
