namespace Arborescence.Traversal.Specialized
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph — implemented as enumerator.
    /// </summary>
    /// <remarks>
    /// This implementation assumes vertices to be indices in range [0, vertexCount).
    /// </remarks>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public readonly partial struct EnumerableDfs<TGraph, TEdge, TEdgeEnumerator>
        where TGraph : IIncidenceGraph<int, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
